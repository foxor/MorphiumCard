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
    public Minion
        Minion;
    
    public override void Apply (string target) {
        SubstitutionExpression.Substitutions[ATTACK_NAME] = Minion.Attack;
        SubstitutionExpression.Substitutions[DEFENSE_NAME] = Minion.Defense;
        Lane lane = GameState.GetLane(target);
        if (lane != null) {
            lane.SpawnFriendly(Minion);
        }
    }
}