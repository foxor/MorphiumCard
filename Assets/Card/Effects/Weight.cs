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
    [ProtoMember(3)]
    public Expression Magnitude;
    
    public override void Apply (string target) {
        SubstitutionExpression.Substitutions[ARGUMENT_NAME] = Magnitude;
        GameState.GetMorphid(target).Weight += Magnitude.Evaluate();
    }
}