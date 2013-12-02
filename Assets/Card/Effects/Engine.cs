using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Engine : Effect {
    public const string CSV_NAME = "Engine";
    public const string ARGUMENT_NAME = "ENGINE";

    [SerializeField]
    [ProtoMember(3)]
    public int
        Magnitude;
    
    public override void Apply (string target) {
        SubstitutionExpression.Substitutions[ARGUMENT_NAME] = Magnitude;
        Morphid morphid = GameState.GetMorphid(target);
        if (morphid != null) {
            morphid.Engine += Magnitude;
        }
    }
}