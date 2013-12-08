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

    public override void Apply(string TargetGuid) {
        this.TargetGuid = TargetGuid;
        GameState.ActiveMorphid.Research = this;
        //Source.Effects = Source.Effects.Where(x => x.
    }

    public void Activate() {
        GameState.ActiveMorphid.Research.Source.Process(
            new string[]{GameState.ActiveMorphid.Research.TargetGuid}
        );
    }
}