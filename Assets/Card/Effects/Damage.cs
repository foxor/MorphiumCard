using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Damage : Effect {
    public const string CSV_NAME = "Damage";
    
    [SerializeField]
    [ProtoMember(1)]
    public int targeting;
    
    [SerializeField]
    [ProtoMember(2)]
    public TargetingType targetingType;
    
    [SerializeField]
    [ProtoMember(3)]
    public string magnitude;
    public int Magnitude {
        get {
            return Parser.Parse(magnitude).EvaluateAsInt();
        }
    }
    
    public override void Apply (string targetGuid) {
        GameState.DamageGuid(targetGuid, Magnitude);
    }
    
    public override int Targeting () {
        return targeting;
    }
    
    public override TargetingType TargetingType () {
        return targetingType;
    }
}
