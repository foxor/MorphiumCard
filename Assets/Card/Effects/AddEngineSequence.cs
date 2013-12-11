using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class AddEngineSequence : Effect {
    public const string CSV_NAME = "AddEngineSequence";

    [SerializeField]
    [ProtoMember(1)]
    public Card Source;

    public override void Apply (string TargetGuid) {
        GameState.ActiveMorphid.EngineSequence = 
            GameState.ActiveMorphid.EngineSequence.Concat(Source.Effects.After(x => x.GetType() == typeof(AddEngineSequence))).ToArray();
    }

    public override int Targeting () {
        return (int)TargetTypeFlag.Friendly & (int)TargetTypeFlag.Morphid;
    }

    public override TargetingType TargetingType () {
        return global::TargetingType.All;
    }
}

public static class AfterExtension {
    public static IEnumerable<TSource> After<TSource>(this IEnumerable<TSource> x, Func<TSource, bool> predicate) {
        bool predicated = false;
        foreach (TSource t in x) {
            if (predicated) {
                yield return t;
            }
            predicated = predicated || predicate(t);
        }
    }
}