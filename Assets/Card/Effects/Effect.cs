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
        case Destroy.CSV_NAME:
            return 0;
        }
        return 1;
    }

    public static EffectWrapper Build (string effect, string[] arguments, int target, TargetingType targetingType) {
        Effect r = null;
        switch (effect) {
        case Damage.CSV_NAME:
            r = new Damage(){magnitude = arguments[0]};
            break;
        case Engine.CSV_NAME:
            r = new Engine(){magnitude = arguments[0]};
            break;
        case Healing.CSV_NAME:
            r = new Healing(){magnitude = arguments[0]};
            break;
        case Reflect.CSV_NAME:
            r = new Reflect(){magnitude = arguments[0]};
            break;
        case Spawn.CSV_NAME:
            r = new Spawn(){
                Attack = arguments[0],
                Defense = arguments[1]
            };
            break;
        case Weight.CSV_NAME:
            r = new Weight(){magnitude = arguments[0]};
            break;
        case Destroy.CSV_NAME:
            r = new Destroy();
            break;
        }
        r.Targeting = target;
        r.TargetingType = targetingType;
        return EffectWrapper.Build(r);
    }
}