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
    
    public static Card[] Cards {
        get {
            return LocalPlayer.CardContainer.Cards;
        }
    }
    
    [HideInInspector]
    [SerializeField]
    [ProtoMember(2)]
    public string GUID;

    [SerializeField]
    [ProtoMember(3)]
    public int Health;

    [SerializeField]
    [ProtoMember(4)]
    public int Morphium;

    [SerializeField]
    [ProtoMember(5)]
    public int Engine;

    [SerializeField]
    [ProtoMember(6)]
    public int Reflect;

    [SerializeField]
    [ProtoMember(7)]
    public int Weight;

    [SerializeField]
    [ProtoMember(8)]
    public Research Research;
    
    [SerializeField]
    [ProtoMember(9)]
    public EffectWrapper[] EngineSequence;
    
    [SerializeField]
    [ProtoMember(10)]
    public int Prevent;
    
    public Morphid () {
    }

    public Morphid Setup() {
        Health = MAX_HEALTH;
        Morphium = START_MORPHIUM;
        Engine = START_ENGINE;
        Reflect = 0;
        Weight = 0;
        EngineSequence = new EffectWrapper[0];
        return this;
    }
    
    public static void PlayLocalCard (int card, string targetGuid) {
        Client.PlayCard(Morphid.Cards[card], targetGuid);
    }
    
    public void PlayCard (string cardGuid, string pickedGuid) {
        Card c = CardContainer.FromGuid(cardGuid);
        c.Process(pickedGuid);
        CardContainer.ReplaceCard(c);
        CardContainer.Uncharge();
        CardContainer.ComboManufacturer(c.Manufacturer);
        CardContainer.ComboSlot(c.Slot);
    }
}