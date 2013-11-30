using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class MinionAnimation : SignalData {
    [SerializeField]
    [ProtoMember(1)]
    public AnimationType
        AnimationType;
    [SerializeField]
    [ProtoMember(2)]
    public string
        GUID;
    protected Animation animation;
    
    public override void OnActivate () {
        animation = GameState.GetMinion(GUID).GameObject.GetComponent<Animation>();
        animation.Play(AnimationType.ToString());
    }
    
    public override bool IsActive () {
        return animation.IsPlaying(AnimationType.ToString());
    }
}