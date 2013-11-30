﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Spawn : Effect {
    [SerializeField]
    [ProtoMember(1)]
    public Minion
        Minion;
    
    public override void Apply (string target) {
        Lane lane = GameState.GetLane(target);
        if (lane != null) {
            lane.SpawnFriendly(Minion);
        }
    }
}