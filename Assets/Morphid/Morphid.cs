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
    
    [SerializeField]
    [ProtoMember(2)]
    public int
        Health;
    [SerializeField]
    [ProtoMember(3)]
    public int
        Morphium;
    [SerializeField]
    [ProtoMember(4)]
    public int
        Engine;
    [SerializeField]
    [ProtoMember(5)]
    public int
        Reflect;
    [HideInInspector]
    [SerializeField]
    [ProtoMember(6)]
    public string
        GUID;
    
    public Morphid () {
        Health = MAX_HEALTH;
        Morphium = START_MORPHIUM;
        Engine = START_ENGINE;
        Reflect = 0;
    }
    
    public static void PlayLocalCard (int card, string targetGuid) {
        Client.PlayCard(Morphid.Cards[card], targetGuid);
    }
    
    public void PlayCard (string cardGuid, string targetGuid) {
        Card c = CardContainer.FromGuid(cardGuid);
        TargetingRequirements req = new TargetingRequirements (c.Effects[0].Targeting, c.Effects[0].TargetingType);
        switch (c.Effects[0].TargetingType) {
        case TargetingType.Single:
            if (req.TargetAllowed(targetGuid)) {
                c.Process(new string[] {targetGuid});
            } else {
                Debug.Log("Client picked impossible target!");
            }
            break;
        case TargetingType.All:
            c.Process(req.AllTargets(targetGuid).ToArray());
            break;
        }
        CardContainer.ReplaceCard(c);
        CardContainer.Uncharge();
        CardContainer.ComboManufacturer(c.Manufacturer);
        CardContainer.ComboSlot(c.Slot);
    }
}