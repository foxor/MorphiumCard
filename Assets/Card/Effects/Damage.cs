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
    public const string ARGUMENT_NAME = "DAMAGE";
    
    [SerializeField]
    [ProtoMember(3)]
    public string magnitude;
    public int Magnitude {
        get {
            return Expression.Parse(magnitude).Evaluate();
        }
    }
    
    public override void Apply (string targetGuid) {
        SubstitutionExpression.Substitutions[ARGUMENT_NAME] = Magnitude;
        GameState.DamageGuid(targetGuid, Magnitude);
    }
}
