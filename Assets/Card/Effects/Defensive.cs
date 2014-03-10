using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Defensive {
    public const string CSV_NAME = "Defensive";

    public  void Apply (string TargetGuid) {
    }

    public  int Targeting () {
        return 0;
    }

    public  TargetingType TargetingType () {
        return global::TargetingType.Skip;
    }
}