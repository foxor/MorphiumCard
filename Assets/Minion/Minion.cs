using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

public class MinionBuilder {
    public bool Defensive = false;
    public bool Protect = false;
    public bool Scrounge = false;
    public bool OnFire = false;
    public bool Blitz = false;
    public bool Hazmat = false;
}

[Serializable]
[ProtoContract]
public class Minion {
    
    [SerializeField]
    [ProtoMember(1)]
    public string GUID;

    [SerializeField]
    [ProtoMember(2)]
    public int InitialAttack;
    
    [SerializeField]
    [ProtoMember(3)]
    public int Defense;

    [SerializeField]
    [ProtoMember(4)]
    public bool HasTriedToAttack;

    [SerializeField]
    [ProtoMember(5)]
    public string MorphidGUID;

    [SerializeField]
    [ProtoMember(6)]
    public bool Defensive;
    
    [SerializeField]
    [ProtoMember(7)]
    public bool Protect;
    
    [SerializeField]
    [ProtoMember(8)]
    public bool Scrounge;
    
    [SerializeField]
    [ProtoMember(9)]
    public bool OnFire;
    
    [SerializeField]
    [ProtoMember(10)]
    public bool Blitz;
    
    [SerializeField]
    [ProtoMember(11)]
    public bool Hazmat;

    public GameObject GameObject;
    
    public bool IsFriendly (string morphidGuid) {
        return morphidGuid == MorphidGUID;
    }
    
    public bool IsEnemy (string morphidGuid) {
        return morphidGuid != MorphidGUID;
    }
    
    public static bool IsDead (Minion minion) {
        return minion == null || minion.Defense <= 0;
    }

    public bool AffectedByTerrain(TerrainType terrainType) {
        return GameState.GetLane(this).isTerrainType(terrainType) && !Hazmat && !GameState.GetMorphid(MorphidGUID).IgnoreTerrain;
    }

    public int Attack {
        get {
            return Mathf.Max(InitialAttack - (AffectedByTerrain(TerrainType.Flooded) ? 2 : 0), 0);
        }
    }
    
    public Minion () {
        GUID = Guid.NewGuid().ToString();
    }

    public void DoAttack () {
        if (!HasTriedToAttack) {
            HasTriedToAttack = true;
            if (AffectedByTerrain(TerrainType.Sticky)) {
                return;
            }
        }

        if (Scrounge) {
            GameState.AddParts(GameState.ActiveMorphid.GUID, 1);
        }
        
        Lane myLane = GameState.GetLane(this);
        int laneIndex = GameState.GetLaneIndex(myLane);
        Minion defender = GameState.GetLaneDefender(laneIndex);
        if (defender != null) {
            AnimationSignalManager.SendRPC(
                AnimationSignalManager.Singleton.QueueMinionAnimation,
                new MinionAnimation() {
                AnimationType = AnimationType.AttackMinion,
                GUID = GUID
            }
            );
            AnimationSignalManager.SendRPC(
                AnimationSignalManager.Singleton.QueueMinionHealthLie,
                new MinionHealthLie() {
                GUID = defender.GUID,
                Health = defender.Defense
            }
            );
            AnimationSignalManager.SendRPC(
                AnimationSignalManager.Singleton.QueueMinionAliveLie,
                new MinionAliveLie() {
                GUID = defender.GUID,
                Alive = true
            }
            );
            
            GameState.DamageGuid(defender.GUID, GUID, Attack);
        }
        else {
            if (!Defensive) {
                GameState.DamageGuid(GameState.InactiveMorphid.GUID, GUID, Attack);
            }
        }
    }

    public void OnTurnBegin () {
        if (OnFire) {
            GameState.DamageGuid(GUID, GUID, 1);
        }

        bool attack = MorphidGUID == GameState.ActiveMorphid.GUID;
        if (attack) {
            DoAttack();
            if (AffectedByTerrain(TerrainType.Acid)) {
                GameState.DamageGuid(GUID, GUID, 2);
            }
        }
    }

    public void OnSpawn () {
        if (Blitz || AffectedByTerrain(TerrainType.Slick)) {
            DoAttack();
        }
    }
}