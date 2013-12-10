using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class EffectWrapper {
    [SerializeField]
    [ProtoMember(1)]
    public Damage Damage;
    
    [SerializeField]
    [ProtoMember(2)]
    public Destroy Destroy;
    
    [SerializeField]
    [ProtoMember(3)]
    public Engine Engine;
    
    [SerializeField]
    [ProtoMember(4)]
    public Healing Healing;
    
    [SerializeField]
    [ProtoMember(5)]
    public Reflect Reflect;
    
    [SerializeField]
    [ProtoMember(6)]
    public Spawn Spawn;
    
    [SerializeField]
    [ProtoMember(7)]
    public Weight Weight;

    [SerializeField]
    [ProtoMember(8)]
    public Research Research;

    public static EffectWrapper Build(Effect e) {
        EffectWrapper r = new EffectWrapper();
        if (e.GetType() == typeof(Damage)) {
            r.Damage = (Damage)e;
        }
        if (e.GetType() == typeof(Destroy)) {
            r.Destroy = (Destroy)e;
        }
        if (e.GetType() == typeof(Engine)) {
            r.Engine = (Engine)e;
        }
        if (e.GetType() == typeof(Healing)) {
            r.Healing = (Healing)e;
        }
        if (e.GetType() == typeof(Reflect)) {
            r.Reflect = (Reflect)e;
        }
        if (e.GetType() == typeof(Spawn)) {
            r.Spawn = (Spawn)e;
        }
        if (e.GetType() == typeof(Weight)) {
            r.Weight = (Weight)e;
        }
        if (e.GetType() == typeof(Research)) {
            r.Research = (Research)e;
        }
        return r;
    }

    public Effect Wrapped {
        get {
            if (Damage != null) {
                return Damage;
            }
            if (Destroy != null) {
                return Destroy;
            }
            if (Engine != null) {
                return Engine;
            }
            if (Healing != null) {
                return Healing;
            }
            if (Reflect != null) {
                return Reflect;
            }
            if (Spawn != null) {
                return Spawn;
            }
            if (Weight != null) {
                return Weight;
            }
            if (Research != null) {
                return Research;
            }
            Debug.Log("Effect wrapper is wrapping nothing");
            return null;
        }
    }

    public void Apply(string target) {
        Wrapped.Apply(target);
    }
}