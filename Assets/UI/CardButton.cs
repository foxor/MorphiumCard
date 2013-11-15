using UnityEngine;
using System.Collections;

public class CardButton : Button {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;
    public int CardIndex;
    protected CardMarker Card;
    protected CostFieldMarker CardCost;
    protected NameFieldMarker CardName;
    protected TextFieldMarker CardText;
    protected int oldLeft;
    protected int oldTop;
    protected float zPos;
    
    public CardButton (int CardIndex, Region source, CardMarker card) : base(source) {
        this.CardIndex = CardIndex;
        Action = UI.Singleton.PickupCard (CardIndex);
        Card = card;
        CardCost = card.GetComponentInChildren<CostFieldMarker> ();
        CardName = card.GetComponentInChildren<NameFieldMarker> ();
        CardText = card.GetComponentInChildren<TextFieldMarker> ();
        Vector3 screenPos = Camera.main.WorldToScreenPoint(Card.transform.position);
        Left = (int)screenPos.x;
        Top = (int)screenPos.y;
        Width = ART_WIDTH;
        Height = ART_HEIGHT;
        zPos = Card.transform.position.z;
    }

    public void OnPickup () {
        oldLeft = Left;
        oldTop = Top;
    }

    public void OnDrop () {
        Left = oldLeft;
        Top = oldTop;
    }

    public bool isEnabled () {
        return Morphid.Cards != null && 
            Morphid.Cards [CardIndex] != null &&
            Morphid.Cards [CardIndex].Cost <= Morphid.LocalPlayer.Morphium;
    }
    
    protected override void DrawInner () {
        Card.transform.position = new Vector3 (Left, Top, zPos);
        if (Morphid.Cards != null && Morphid.Cards [CardIndex] != null) {
            Text = Morphid.Cards [CardIndex].Name +
                " (" + Morphid.Cards [CardIndex].Cost + ")\n" +
                Morphid.Cards [CardIndex].Text;
            CardCost.Text = Morphid.Cards [CardIndex].Cost.ToString ();
            CardName.Text = Morphid.Cards [CardIndex].Name;
            CardText.Text = Morphid.Cards [CardIndex].Text;
        } else {
            Text = "Empty";
        }
        if (ContainsMouse () != null && Input.GetMouseButton (0) && Enabled) {
            Action ();
            invalid = true;
        }
        base.DrawInner ();
    }
}