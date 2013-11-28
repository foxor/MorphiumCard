using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Minion {
    
    [SerializeField]
    [ProtoMember(1)]
    public string
        GUID;
    [SerializeField]
    [ProtoMember(2)]
    public int
        Attack;
    [SerializeField]
    [ProtoMember(3)]
    public int
        Defense;
    [SerializeField]
    [ProtoMember(4)]
    public string
        MorphidGUID;
    public GameObject GameObject;
    
    public bool IsFriendly (string morphidGuid) {
        return morphidGuid == MorphidGUID;
    }
    
    public bool IsEnemy (string morphidGuid) {
        return morphidGuid != MorphidGUID;
    }
    
    public static bool IsDead (Minion minion) {
        return minion == null || minion.Defense == 0;
    }
    
    public Minion () {
        GUID = Guid.NewGuid().ToString();
    }
}