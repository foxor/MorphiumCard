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
    [ProtoMember(3)]
    public Expression Attack;

    [SerializeField]
    [ProtoMember(4)]
    public Expression Defense;
    
    public override void Apply (string target) {
        int attack = Attack.Evaluate();
        int defense = Defense.Evaluate();
        SubstitutionExpression.Substitutions[ATTACK_NAME] = attack;
        SubstitutionExpression.Substitutions[DEFENSE_NAME] = defense;
        Lane lane = GameState.GetLane(target);
        if (lane != null) {
            lane.SpawnFriendly(new Minion(){Attack = attack, Defense = defense});
        }
    }
}