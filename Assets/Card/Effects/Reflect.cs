﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Reflect : Effect {
    [SerializeField]
    [ProtoMember(1)]
    public int
        Magnitude;
    
    public override void Apply (string target) {
        Morphid morphid = GameState.GetMorphid(target);
        if (morphid != null) {
            morphid.Reflect += Magnitude;
        }
    }
}