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
    [ProtoMember(3)]
    public int
        Magnitude;
    
    public override void Apply (string target) {
        GameState.GetMorphid(target).Weight += Magnitude;
    }
}