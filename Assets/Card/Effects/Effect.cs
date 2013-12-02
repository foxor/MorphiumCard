using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public abstract class Effect {
    [SerializeField]
    [ProtoMember(1)]
    public int
        Targeting;
    [SerializeField]
    [ProtoMember(2)]
    public TargetingType
        TargetingType;

    public abstract void Apply (string TargetGuid);

    public static int Arguments (string effect) {
        switch (effect) {
        case Spawn.CSV_NAME:
            return 2;
        }
        return 1;
    }

    public static Effect Build (string effect, string[] arguments, int target, TargetingType targetingType) {
        Effect r = null;
        switch (effect) {
        case Damage.CSV_NAME:
            r = new Damage(){Magnitude = int.Parse(arguments[0])};
            break;
        case Engine.CSV_NAME:
            r = new Engine(){Magnitude = int.Parse(arguments[0])};
            break;
        case Healing.CSV_NAME:
            r = new Healing(){Magnitude = int.Parse(arguments[0])};
            break;
        case Reflect.CSV_NAME:
            r = new Reflect(){Magnitude = int.Parse(arguments[0])};
            break;
        case Spawn.CSV_NAME:
            r = new Spawn(){Minion = new Minion(){
                        Attack = int.Parse(arguments[0]),
                        Defense = int.Parse(arguments[1])
                    }
                };
            break;
        }
        r.Targeting = target;
        r.TargetingType = targetingType;
        return r;
    }
}