using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Weight : Effect {
    public const string CSV_NAME = "Weight";

    [SerializeField]
    [ProtoMember(1)]
    public string magnitude;
    public int Magnitude {
        get {
            return Parser.Parse(magnitude).EvaluateAsInt();
        }
    }
    
    public override void Apply (string target) {
        GameState.ActiveMorphid.Weight += Magnitude;
    }
    
    public override int Targeting () {
        return 0;
    }
    
    public override TargetingType TargetingType () {
        return global::TargetingType.Skip;
    }
}