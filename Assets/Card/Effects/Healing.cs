using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Healing : Effect {
    [SerializeField]
    [ProtoMember(1)]
    public int
        Magnitude;
    
    public override void Apply (string target) {
        GameState.HealGuid(target, Magnitude);
    }
}