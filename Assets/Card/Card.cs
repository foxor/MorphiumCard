using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public sealed class Card {
    [SerializeField]
    public Slot
        Slot;
    [SerializeField]
    public int
        Appearances;
    [NonSerialized]
    //Prevents unity from copying the guids around
    [ProtoMember(1)]
    public string
        GUID;
    [SerializeField]
    [ProtoMember(2)]
    public String
        Name;
    [SerializeField]
    [ProtoMember(3)]
    public String
        Text;
    [SerializeField]
    [ProtoMember(4)]
    public String
        Manufacturer;
    [SerializeField]
    [ProtoMember(5)]
    public int
        Cost;
    [SerializeField]
    [ProtoMember(6)]
    public int
        Targeting;
    [SerializeField]
    [ProtoMember(7)]
    public TargetingType
        TargetingType;
    [SerializeField]
    [ProtoMember(8)]
    public Damage
        Damage;
    [SerializeField]
    [ProtoMember(9)]
    public Healing
        Healing;
    [SerializeField]
    [ProtoMember(10)]
    public Reflect
        Reflect;
    [SerializeField]
    [ProtoMember(11)]
    public Engine
        Engine;
    [SerializeField]
    [ProtoMember(12)]
    public Spawn
        Spawn;
    [SerializeField]
    [ProtoMember(13)]
    public bool
        Charged;
    
    public Card () {
        GUID = Guid.NewGuid().ToString();
        Damage = new Damage ();
        Healing = new Healing ();
        Reflect = new Reflect ();
        Engine = new Engine ();
        Spawn = new Spawn ();
    }
    
    public void Process (string[] targetGuids) {
        Morphid self = GameState.ActiveMorphid;
        if (self.Morphium >= Cost) {
            self.Morphium -= Cost;
            foreach (string targetGuid in targetGuids) {
                Damage.Apply(targetGuid);
                Healing.Apply(targetGuid);
                Reflect.Apply(targetGuid);
                Engine.Apply(targetGuid);
                Spawn.Apply(targetGuid);
            }
        }
    }

    public Card Copy() {
        return this.SerializeProtoBytes().DeserializeProtoBytes<Card>();
    }
}