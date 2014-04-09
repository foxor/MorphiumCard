using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class CardButton : SpriteButton {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;

    [NonSerialized]
    public bool SuspendDrag;

    [NonSerialized]
    public Card Card;

    [NonSerialized]
    public Morphid Owner;

    public int CardIndex;
    public TextMeshController CardCost;
    public TextMeshController CardName;

    protected Vector3 oldPos;
    protected Vector3 delta;
    protected bool selected;

    public Card CardFn {
        get {
            if (Card != null || Morphid.LocalPlayer == null || Morphid.Cards == null) {
                return Card;
            }
            return Morphid.Cards[CardIndex];
        }
    }

    public void OnPickup () {
        oldPos = Sprite.transform.position;
        delta = oldPos - Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        selected = true;
    }

    public void OnDrop () {
        Sprite.transform.position = oldPos;
        selected = false;
    }

    public void SnapToMouse() {
        Sprite.transform.position += Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        OnPickup();
    }

    public bool isEnabled () {
        return CardFn != null &&
            CardFn.Cost <= Owner.Morphium &&
            CardFn.Charged &&
            GameState.IsLocalActive;
    }
    
    public override void Update () {
        Enabled = isEnabled();
        if (selected && !SuspendDrag) {
            Sprite.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin + delta;
        }
        if (SuspendDrag) {
            Sprite.transform.position = oldPos;
        }
        base.Update();
    }

    protected override void ManageText () {
        if (CardFn != null) {
            CardCost.Text = CardFn.Cost.ToString();
            CardName.Text = CardFn.Name;
            TextArea.Text = CardFn.Text;
            Sprite.renderer.enabled = true;
        } else {
            CardCost.Text = "";
            CardName.Text = "";
            TextArea.Text = "";
            Sprite.renderer.enabled = false;
        }
        CardCost.renderer.material.color = Sprite.renderer.material.color;
        CardName.renderer.material.color = Sprite.renderer.material.color;
        TextArea.renderer.material.color = Sprite.renderer.material.color;
    }
}