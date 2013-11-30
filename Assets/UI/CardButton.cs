using UnityEngine;
using System.Collections;

public class CardButton : SpriteButton {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;
    public int CardIndex;
    public bool SuspendDrag;
    protected CardMarker Card;
    protected CostFieldMarker CardCost;
    protected NameFieldMarker CardName;
    protected TextFieldMarker CardText;
    protected Vector3 oldPos;
    protected Vector3 delta;
    protected bool selected;
    
    public CardButton (int CardIndex, CardMarker card) : base(card.gameObject) {
        this.CardIndex = CardIndex;
        Action = UI.Singleton.PickupCard(CardIndex);
        Card = card;
        CardCost = card.GetComponentInChildren<CostFieldMarker>();
        CardName = card.GetComponentInChildren<NameFieldMarker>();
        CardText = card.GetComponentInChildren<TextFieldMarker>();
    }

    public void OnPickup () {
        oldPos = Card.transform.position;
        delta = oldPos - Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        selected = true;
    }

    public void OnDrop () {
        Card.transform.position = oldPos;
        selected = false;
    }

    public bool isEnabled () {
        return Morphid.Cards != null && 
            Morphid.Cards[CardIndex] != null &&
            Morphid.Cards[CardIndex].Cost <= Morphid.LocalPlayer.Morphium &&
            Morphid.Cards[CardIndex].Charged &&
            GameState.IsLocalActive;
    }
    
    public override void Update () {
        Enabled = isEnabled();
        if (selected && !SuspendDrag) {
            Card.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin + delta;
        }
        if (SuspendDrag) {
            Card.transform.position = oldPos;
        }

        if (Morphid.Cards != null && Morphid.Cards[CardIndex] != null) {
            CardCost.Text = Morphid.Cards[CardIndex].Cost.ToString();
            CardName.Text = Morphid.Cards[CardIndex].Name;
            CardText.Text = Morphid.Cards[CardIndex].Text;
            Card.renderer.enabled = true;
        } else {
            CardCost.Text = "";
            CardName.Text = "";
            CardText.Text = "";
            Card.renderer.enabled = false;
        }
        base.Update();
        CardCost.renderer.material.color = Sprite.renderer.material.color;
        CardName.renderer.material.color = Sprite.renderer.material.color;
        CardText.renderer.material.color = Sprite.renderer.material.color;
    }
}