using UnityEngine;
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
    public const string ATTACK_NAME = "ATTACK";
    public const string DEFENSE_NAME = "DEFENSE";
    
    [SerializeField]
    [ProtoMember(1)]
    public int targeting;
    
    [SerializeField]
    [ProtoMember(2)]
    public TargetingType targetingType;
    
    [SerializeField]
    [ProtoMember(3)]
    public string Attack;

    [SerializeField]
    [ProtoMember(4)]
    public string Defense;
    
    public override void Apply (string target) {
        int attack = Expression.Parse(Attack).Evaluate();
        int defense = Expression.Parse(Defense).Evaluate();
        SubstitutionExpression.Substitutions[ATTACK_NAME] = attack;
        SubstitutionExpression.Substitutions[DEFENSE_NAME] = defense;
        Lane lane = GameState.GetLane(target);
        if (lane != null) {
            lane.SpawnFriendly(new Minion(){Attack = attack, Defense = defense});
        }
    }
    
    public override int Targeting () {
        return targeting;
    }
    
    public override TargetingType TargetingType () {
        return targetingType;
    }
}