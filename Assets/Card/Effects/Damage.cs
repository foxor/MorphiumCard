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
    [ProtoMember(3)]
    public int
        Magnitude;
    
    public override void Apply (string targetGuid) {
        GameState.DamageGuid(targetGuid, Magnitude);
    }
}
