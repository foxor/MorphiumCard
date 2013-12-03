using UnityEngine;
using System.Collections;
using System.Linq;

public class SubstituteText {
    public readonly char[] DELIMITERS = {'{','}'};

    protected string orig;
    protected string lastParse;

    public string Parse(string orig) {
        if (orig == this.orig) {
            return lastParse;
        }
        return lastParse = orig
            .Split(DELIMITERS)
            .Select((x, i) => i % 2 == 1 ? new SubstitutionExpression(x).Evaluate().ToString() : x)
            .Aggregate((x, y) => x + y);
    }
}

public class CardButton : SpriteButton {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;
    public int CardIndex;
    public bool SuspendDrag;
    protected CardMarker Card;
    protected CostFieldMarker CardCost;
    protected NameFieldMarker CardName;
    protected TextFieldMarker CardText;
    
    protected SubstituteText CostText;
    protected SubstituteText NameText;
    protected SubstituteText MainText;

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
        CostText = new SubstituteText();
        NameText = new SubstituteText();
        MainText = new SubstituteText();
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
            CardCost.Text = CostText.Parse(Morphid.Cards[CardIndex].Cost.ToString());
            CardName.Text = NameText.Parse(Morphid.Cards[CardIndex].Name);
            CardText.Text = MainText.Parse(Morphid.Cards[CardIndex].Text);
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