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
    public const string CSV_NAME = "Heal";
    public const string ARGUMENT_NAME = "HEAL";
    
    [SerializeField]
    [ProtoMember(3)]
    public Expression Magnitude;
    
    public override void Apply (string target) {
        SubstitutionExpression.Substitutions[ARGUMENT_NAME] = Magnitude;
        GameState.HealGuid(target, Magnitude.Evaluate());
    }
}