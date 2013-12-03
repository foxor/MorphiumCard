using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Destroy : Effect {
    public const string CSV_NAME = "Destroy";

    [SerializeField]
    [ProtoMember(1)]
    public int targeting;
    
    [SerializeField]
    [ProtoMember(2)]
    public TargetingType targetingType;

    public override int Targeting () {
        return targeting;
    }

    public override TargetingType TargetingType () {
        return targetingType;
    }
    
    public override void Apply (string target) {
        SubstitutionExpression.Substitutions[CSV_NAME] = GameState.GetMinion(target);
        GameState.RemoveMinion(target);
    }
}