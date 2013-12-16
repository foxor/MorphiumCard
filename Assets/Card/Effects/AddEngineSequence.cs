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
        IEnumerable<EffectWrapper> sequenceAddition = Source.Effects.After(x => x.GetType() == typeof(AddEngineSequence));
        if (GameState.ActiveMorphid.EngineSequence == null) {
            GameState.ActiveMorphid.EngineSequence = sequenceAddition.ToArray();
        }
        else {
            GameState.ActiveMorphid.EngineSequence = 
                GameState.ActiveMorphid.EngineSequence.Concat(sequenceAddition).ToArray();
        }
    }

    public override int Targeting () {
        return 0;
    }

    public override TargetingType TargetingType () {
        return global::TargetingType.Skip;
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