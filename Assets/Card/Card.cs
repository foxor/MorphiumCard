﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public sealed class Card {
    [SerializeField]
    public Slot Slot;

    [SerializeField]
    public int Appearances;

    [NonSerialized]
    //Prevents unity from copying the guids around
    [ProtoMember(1)]
    public string GUID;

    [SerializeField]
    [ProtoMember(2)]
    public String Name;

    [SerializeField]
    [ProtoMember(3)]
    public String Text;

    [SerializeField]
    [ProtoMember(4)]
    public String Manufacturer;

    [SerializeField]
    [ProtoMember(5)]
    public int  Cost;

    [SerializeField]
    [ProtoMember(6)]
    public EffectWrapper[] Effects;

    [SerializeField]
    [ProtoMember(11)]
    public bool Charged;
    
    public Card () {
        GUID = Guid.NewGuid().ToString();
        Effects = new EffectWrapper[0];
    }
    
    public void Process (string[] targetGuids) {
        Morphid self = GameState.ActiveMorphid;
        if (self.Morphium >= Cost) {
            self.Morphium -= Cost;
            foreach (string targetGuid in targetGuids) {
                foreach (EffectWrapper effect in Effects) {
                    effect.Apply(targetGuid);
                }
            }
        }
    }

    public void Bind() {
        SubstitutionExpression.cardContext = this;
    }

    public Card Copy() {
        return this.SerializeProtoBytes().DeserializeProtoBytes<Card>();
    }

    private IEnumerable<EffectWrapper> BuildInner(string[] effects, string[] arguments, int[] targets, TargetingType[] targetTypes) {
        IEnumerable<string> args = arguments.AsEnumerable();
        int argPtr = 0;
        for (int i = 0; i < effects.Length; i++) {
            string effect = effects[i];
            int argCount = Effect.Arguments(effect);
            yield return Effect.Build(
                effect, 
                args.Skip(argPtr).Take(argCount).ToArray(), 
                targets.Length == 1 ? targets[0] : targets[i],
                targetTypes.Length == 1 ? targetTypes[0] : targetTypes[i],
                this
            );
            argPtr += argCount;
        }
    }

    public IEnumerable<T> EffectsOfType<T>() {
        return Effects.Select(x => x.Wrapped).Where(x => x.GetType() == typeof(T)).Cast<T>();
    }

    public void Build(string[] effects, string[] arguments, int[] targets, TargetingType[] targetTypes) {
        this.Effects = BuildInner(effects, arguments, targets, targetTypes).ToArray();
        foreach (Effect e in Effects.Select(x => x.Wrapped)) {
            e.OnComplete(this);
        }
    }
}