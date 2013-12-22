using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Research : Effect {
    public const string CSV_NAME = "Research";
    
    [SerializeField]
    [ProtoMember(1)]
    public Card Source;

    [SerializeField]
    [ProtoMember(2)]
    public int targeting;

    [SerializeField]
    [ProtoMember(3)]
    public string TargetGuid;

    public override void OnComplete (Card Source) {
        EffectWrapper[] orig = Source.Effects;
        Source.Effects = Source.Effects.After(x => x.Wrapped.GetType() == typeof(Research)).ToArray();
        this.Source = Source.Copy();
        Source.Effects = orig;
    }

    public override void Apply(string TargetGuid) {
        GameState.ActiveMorphid.Research = this;
    }

    public void Activate() {
        Source.Cost = GameState.ActiveMorphid.Morphium;
        try {
            Source.Process("");
        }
        catch (TargetingException e) {
            Debug.Log("Research targeting error suppressed: " + e.Message);
        }
    }
    
    public override int Targeting () {
        return targeting;
    }
    
    public override TargetingType TargetingType () {
        return global::TargetingType.Skip;
    }

    public override bool IgnoreAfter() {
        return true;
    }
}