using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Defensive : Effect {
    public const string CSV_NAME = "Defensive";

    public override void Apply (string TargetGuid) {
    }

    public override int Targeting () {
        return 0;
    }

    public override TargetingType TargetingType () {
        return global::TargetingType.Skip;
    }
}