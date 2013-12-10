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

    public abstract void Apply(string TargetGuid);

    public static int Arguments(string effect) {
        switch(effect) {
        case Spawn.CSV_NAME:
            return 2;
        case Destroy.CSV_NAME:
            return 0;
        }
        return 1;
    }

    public static EffectWrapper Build(string effect, string[] arguments, int target, TargetingType targetingType, Card card) {
        Effect r = null;
        switch(effect) {
        case Damage.CSV_NAME:
            r = new Damage(){
                magnitude = arguments[0],
                targeting = target,
                targetingType = targetingType
            };
            break;
        case Engine.CSV_NAME:
            r = new Engine(){
                magnitude = arguments[0],
                targeting = target,
                targetingType = targetingType
            };
            break;
        case Healing.CSV_NAME:
            r = new Healing(){
                magnitude = arguments[0],
                targeting = target,
                targetingType = targetingType
            };
            break;
        case Reflect.CSV_NAME:
            r = new Reflect(){
                magnitude = arguments[0],
                targeting = target,
                targetingType = targetingType
            };
            break;
        case Spawn.CSV_NAME:
            r = new Spawn(){
                attack = arguments[0],
                defense = arguments[1],
                targeting = target,
                targetingType = targetingType
            };
            break;
        case Weight.CSV_NAME:
            r = new Weight(){
                magnitude = arguments[0],
                targeting = target,
                targetingType = targetingType
            };
            break;
        case Destroy.CSV_NAME:
            r = new Destroy(){
                targeting = target,
                targetingType = targetingType
            };
            break;
        case Research.CSV_NAME:
            r = new Research() {
                targeting = target,
                Source = card
            };
            break;
        default:
            Debug.Log("Effect not implemented: " + effect);
            break;
        }
        return EffectWrapper.Build(r);
    }
    
    public abstract int Targeting();

    public abstract TargetingType TargetingType();
}