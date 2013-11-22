using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour {
    protected const int DRAG_SIZE = 5;
    protected const float DRAG_TIMER = 0.3f;

    public static UI Singleton;

    public GameObject LeftSide;
    public GameObject EngineButton;

    protected CardButton[] Cards;
    protected CardButton Selected;
    protected SpriteButton EngineSprite;
    protected SpriteButton DrawSprite;

    protected Target Target;
    protected TargetingRequirements CardRequirements;
    protected TargetingMode TargetingMode;

    protected List<SpriteRegion> Sprites;
    
    public void Awake () {
        Singleton = this;

        Sprites = new List<SpriteRegion>();

        Cards = new CardButton[4];

        CardMarker[] CardComponents = FindObjectsOfType<CardMarker>().OrderBy(
            x => x.transform.position.x * 4000 + x.transform.position.y
        ).ToArray();
        Cards[0] = new CardButton (0, CardComponents[0]);
        Cards[1] = new CardButton (1, CardComponents[1]);
        Cards[2] = new CardButton (2, CardComponents[2]);
        Cards[3] = new CardButton (3, CardComponents[3]);
        Sprites.AddRange(Cards);

        EngineSprite = new SpriteButton(EngineButton) {
            Action = Client.BoostEngine
        };
        Sprites.Add(EngineSprite);
    }
    
    public void Start () {
        Target = new Target ();
        Target.Draw(Sprites);
    }
    
    public Action PickupCard (int card) {
        return () => {
            if (Selected != null || TargetingMode != TargetingMode.Inactive) {
                return;
            }
            TargetingMode = TargetingMode.Transitional;
            Selected = Cards[card];
            Selected.OnPickup();
            CardRequirements = new TargetingRequirements (Morphid.Cards[card].Targeting, Morphid.Cards[card].TargetingType);
            StartCoroutine(Select(card));
        };
    }
    
    protected IEnumerator Select (int card) {
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
            if (CardRequirements.TargetingType == TargetingType.All || ClickRaycast.MouseOverThis(LeftSide)) {
                ReticleController.Shown = false;
                Selected.SuspendDrag = false;
            } else {
                ReticleController.Shown = true;
                Selected.SuspendDrag = true;
            }
        }
        Target.SetTarget(Sprites
                         .Where(x => typeof(SelectionRegion).IsAssignableFrom(x.GetType()))
                         .Cast<SelectionRegion>()
                         .Where(x => x.ContainsMouse())
                         .SingleOrDefault()
        );
        if (
            CardRequirements.AllTargets(Target.GUID).Count() > 0 && 
            !cancel &&
            Morphid.Cards[card].Cost <= Morphid.LocalPlayer.Morphium
        ) {
            Morphid.PlayLocalCard(card, Target.GUID);
        }
        Selected.SuspendDrag = false;
        ReticleController.Shown = false;
        TargetingMode = TargetingMode.Inactive;
        CardRequirements = null;
        Selected.OnDrop();
        Selected = null;
        Cards[card].Enabled = true;
    }
    
    public void Update () {
        if (Morphid.LocalPlayer == null) {
            return;
        }
        EngineSprite.Text = Morphid.LocalPlayer.Morphium + "/" + Morphid.MAX_MORPHIUM + " (" + Morphid.LocalPlayer.Engine + ")";
        DrawSprite.Enabled = EngineSprite.Enabled = GameState.IsLocalActive;
        foreach (SpriteRegion Sprite in Sprites) {
            Sprite.Update();
        }
        Target.Update(CardRequirements);
    }
}