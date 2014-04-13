using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Morphid {
    
    public const int MAX_HEALTH = 50;
    public const int MAX_MORPHIUM = 10;
    public const int MAX_ENGINE = 10;
    public const int START_MORPHIUM = MAX_MORPHIUM;
    public const int START_ENGINE = 3;
    
    public static Morphid LocalPlayer {
        get {
            return GameState.GetMorphid(Client.GUID);
        }
    }

    public static Morphid RemotePlayer {
        get {
            return GameState.GetEnemy(Client.GUID);
        }
    }
    
    [SerializeField]
    [ProtoMember(1)]
    public CardContainer
        CardContainer = new CardContainer ();
    
    [SerializeField]
    [ProtoMember(2)]
    public AttachmentContainer
        AttachmentContainer = new AttachmentContainer ();
    
    public static Card[] Cards {
        get {
            return LocalPlayer.CardContainer.Cards;
        }
    }
    
    [HideInInspector]
    [SerializeField]
    [ProtoMember(3)]
    public string GUID;

    [SerializeField]
    [ProtoMember(4)]
    public int Health;

    [SerializeField]
    [ProtoMember(5)]
    public int Morphium;

    [SerializeField]
    [ProtoMember(6)]
    public int Engine;

    [SerializeField]
    [ProtoMember(7)]
    public int Reflect;

    [SerializeField]
    [ProtoMember(8)]
    public int Weight;
    
    [SerializeField]
    [ProtoMember(9)]
    public int Parts;
    
    [SerializeField]
    [ProtoMember(10)]
    public bool OnFire;

    public Action Research;
    public Action EngineSequence;
    
    public Morphid () {
    }

    public Morphid Setup() {
        Health = MAX_HEALTH;
        Morphium = START_MORPHIUM;
        Engine = START_ENGINE;
        Reflect = 0;
        Weight = 0;
        EngineSequence = ChargeAll;
        return this;
    }

    protected void ChargeAll() {
        foreach (Slot slot in Enum.GetValues(typeof(Slot))) {
            CardContainer.ComboSlot(slot);
        }
    }
    
    public static void PlayLocalCard (Card card, string targetGuid) {
        Client.PlayCard(card.GUID, targetGuid);
    }
    
    public void PlayCard (string cardGuid, string pickedGuid) {
        Card c = CardContainer.FromGuid(cardGuid);
        CardContainer.ReplaceCard(c);
        CardContainer.Uncharge();
        CardContainer.ComboManufacturer(c.Manufacturer);
        CardContainer.ComboSlot(c.Slot);
        c.Process(pickedGuid);
    }

	public void Retemplate() {
        if (CardContainer.Cards == null) {
            return;
        }
		foreach (Card c in CardContainer.Cards) {
            if (c != null) {
                c.Template();
            }
		}
        AttachmentContainer.Retemplate();
	}

    public void OnTurnBegin() {
        if (OnFire) {
            GameState.DamageGuid(GUID, GUID, 1);
        }

        if (GUID == GameState.ActiveMorphid.GUID) {
            GameState.AddMorphium(GUID, Engine);
            Retemplate();
        }
    }
}