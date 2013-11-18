﻿using UnityEngine;
using System.Collections;

public class CardButton : Button {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;
    public int CardIndex;
    protected CardMarker Card;
    protected CostFieldMarker CardCost;
    protected NameFieldMarker CardName;
    protected TextFieldMarker CardText;
    protected Vector3 oldPos;
    
    public CardButton (int CardIndex, Region source, CardMarker card) : base(source) {
        this.CardIndex = CardIndex;
        Action = UI.Singleton.PickupCard (CardIndex);
        Card = card;
        CardCost = card.GetComponentInChildren<CostFieldMarker> ();
        CardName = card.GetComponentInChildren<NameFieldMarker> ();
        CardText = card.GetComponentInChildren<TextFieldMarker> ();
    }

    public void OnPickup () {
        oldPos = Card.transform.position;
    }

    public void OnDrop () {
        Card.transform.position = oldPos;
    }

    public bool isEnabled () {
        return Morphid.Cards != null && 
            Morphid.Cards [CardIndex] != null &&
            Morphid.Cards [CardIndex].Cost <= Morphid.LocalPlayer.Morphium;
    }
    
    protected override void DrawInner () {
        if (Morphid.Cards != null && Morphid.Cards [CardIndex] != null) {
            CardCost.Text = Morphid.Cards [CardIndex].Cost.ToString ();
            CardName.Text = Morphid.Cards [CardIndex].Name;
            CardText.Text = Morphid.Cards [CardIndex].Text;
            Card.renderer.enabled = true;
        } else {
            Card.renderer.enabled = false;
        }
        if (ClickRaycast.ClickedThis(Card.gameObject) && Enabled) {
            Action ();
        }
    }
}