using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour {
    protected const int DRAG_SIZE = 5;
    protected const float DRAG_TIMER = 0.3f;

    protected static Vector3 CARD_FLOAT_CLIP = new Vector3(0f, 0f, 9f);

    public static UI Singleton;

    public GameObject LeftSide;

    public SelectionRegion[] CardRegions;
    public CardButton[] Cards;
    public Target Target;
    public SpriteButton Engine;

    [NonSerialized]
    public CardButton Selected;
	
    protected Card ActiveCard;
    protected TargetingMode TargetingMode;
    protected List<SpriteRegion> Sprites;
    protected bool hasUpdated;
    
    public void Awake () {
        Singleton = this;

        Sprites = new List<SpriteRegion> ();

        Sprites.AddRange(Cards);

        Engine.Action = Client.BoostEngine;
        Sprites.Add(Engine);
    }
    
    public void Start () {
        Target.Prepare(Sprites);
    }
    
    public Action PickupCard (CardButton card) {
        return () => {
            if (Selected != null || TargetingMode != TargetingMode.Inactive) {
                return;
            }
            TargetingMode = TargetingMode.Transitional;
            Selected = card;
            Selected.OnPickup();
            ActiveCard = card.GetCard();
            StartCoroutine(Select());
        };
    }
    
    protected IEnumerator Select () {
        Region testRegion = new Region () {
            Left = (int)Input.mousePosition.x - DRAG_SIZE,
            Top = Screen.height - (int)Input.mousePosition.y - DRAG_SIZE,
            Width = DRAG_SIZE * 2,
            Height = DRAG_SIZE * 2
        };
        float startTime = DRAG_TIMER;
        
        while (true) {
            startTime -= Time.deltaTime;
            if (!Input.GetMouseButton(0)) {
                TargetingMode = TargetingMode.ClickTargeting;
                break;
            }
            if (testRegion.ContainsMouse() == null || startTime <= 0) {
                TargetingMode = TargetingMode.DragTargeting;
                break;
            }
            yield return 0;
        }
        
        bool cancel = false;
        while (
            (Input.GetMouseButton(0) && TargetingMode == TargetingMode.DragTargeting) ||
            (!Input.GetMouseButton(0) && TargetingMode == TargetingMode.ClickTargeting)
        ) {
            if (Input.GetMouseButton(1)) {
                cancel = true;
                break;
            }
            yield return 0;
            if (ActiveCard.TargetingType == TargetingType.All ||
                ClickRaycast.MouseOverThis(LeftSide)
            ) {
                ReticleController.Shown = false;
                Selected.SuspendDrag = false;
            } else {
                ReticleController.Shown = true;
                Selected.SuspendDrag = true;
            }
        }
        if (ClickRaycast.MouseOverThis(LeftSide)) {
            cancel = true;
        }
        foreach (SelectionRegion pickedRegion in Sprites
                 .Where(x => typeof(SelectionRegion).IsAssignableFrom(x.GetType()))
                 .Cast<SelectionRegion>()
                 .Where(x => x.ContainsMouse())
        ) {
            Target.SetTarget(pickedRegion);
            if (
                !cancel &&
                Selected.GetCard().Cost <= Morphid.LocalPlayer.Morphium &&
                ActiveCard.TargetableGuids != null &&
                ((ActiveCard.TargetableGuids.Contains(Target.GUID)) || ActiveCard.TargetingType != TargetingType.Single)
            ) {
                Morphid.PlayLocalCard(Selected.GetCard(), Target.GUID);
                break;
            }
        }
        Selected.SuspendDrag = false;
        ReticleController.Shown = false;
        TargetingMode = TargetingMode.Inactive;
        ActiveCard = null;
        Selected.OnDrop();
        Selected.Enabled = true;
        Selected = null;
    }
    
    public void Update () {
        if (Morphid.LocalPlayer == null) {
            return;
        }
        if (!hasUpdated) {
            hasUpdated = true;
            foreach (CardButton cardButton in Cards) {
                cardButton.Action = PickupCard(cardButton);
                cardButton.Owner = Morphid.LocalPlayer;
            }
        }


        Engine.Text = Morphid.LocalPlayer.Morphium + "/" + Morphid.MAX_MORPHIUM + " (" + Morphid.LocalPlayer.Engine + ")";
        Engine.Enabled = GameState.IsLocalActive;

        for (int i = 0; i < CardRegions.Length; i++) {
            if (!ReticleController.Shown && 
                ((Selected == null && CardRegions[i].ContainsMouse()) ||
                (Selected == Cards[i]))
            ) {
                Cards[i].Sprite.SetActive(true);
                Cards[i].Sprite.transform.localPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin + CARD_FLOAT_CLIP;
            }
            else {
                Cards[i].Sprite.SetActive(false);
            }
        }

        foreach (SpriteRegion Sprite in Sprites) {
            Sprite.Update();
        }
        Target.Update(ActiveCard == null ? null : ActiveCard.TargetableGuids);
    }
}