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
    public const string ARGUMENT_NAME = "WEIGHT";
    
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
            return Expression.Parse(magnitude).Evaluate();
        }
    }
    
    public override void Apply (string target) {

        SubstitutionExpression.Substitutions[ARGUMENT_NAME] = Magnitude;
        GameState.GetMorphid(target).Weight += Magnitude;
    }
    
    public override int Targeting () {
        return targeting;
    }
    
    public override TargetingType TargetingType () {
        return targetingType;
    }
}