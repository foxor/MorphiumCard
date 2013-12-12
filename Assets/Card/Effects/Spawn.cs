﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Spawn : Effect {
    public const string CSV_NAME = "Summon";
    public const string ATTACK_NAME = "Attack";
    public const string DEFENSE_NAME = "Defense";
    
    [SerializeField]
    [ProtoMember(1)]
    public int targeting;
    
    [SerializeField]
    [ProtoMember(2)]
    public TargetingType targetingType;
    
    [SerializeField]
    [ProtoMember(3)]
    public string attack;
    public int Attack {
        get {
            return Expression.Parse(attack).Evaluate();
        }
    }

    [SerializeField]
    [ProtoMember(4)]
    public string defense;
    public int Defense {
        get {
            return Expression.Parse(defense).Evaluate();
        }
    }

    [SerializeField]
    [ProtoMember(5)]
    public bool Defensive;

    public override void OnComplete (Card Source) {
        Defensive = Source.EffectsOfType<Defensive>().Any();
    }
    
    public override void Apply (string target) {
        Lane lane = GameState.GetLane(target);
        if (lane != null) {
            lane.SpawnFriendly(new Minion(){
                Attack = Attack,
                Defense = Defense,
                Defensive = Defensive
            });
        }
    }
    
    public override int Targeting () {
        return targeting;
    }
    
    public override TargetingType TargetingType () {
        return targetingType;
    }
}