using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Destroy {
    public const string CSV_NAME = "Destroy";

    [SerializeField]
    [ProtoMember(1)]
    public int targeting;
    
    [SerializeField]
    [ProtoMember(2)]
    public TargetingType targetingType;

    public  int Targeting () {
        return targeting;
    }

    public  TargetingType TargetingType () {
        return targetingType;
    }
    
    public  void Apply (string target) {
        GameState.RemoveMinion(target);
    }
}