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
    
    public override void Apply (string target) {
        SubstitutionExpression.Substitutions[CSV_NAME] = GameState.GetMinion(target);
        GameState.RemoveMinion(target);
    }
}