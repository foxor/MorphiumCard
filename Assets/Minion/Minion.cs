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
    public int CurrentAttack;
    
    [SerializeField]
    [ProtoMember(3)]
    public int CurrentDurability;

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

    public Action OnDeath = () => {};

    public GameObject GameObject;
    protected DamageProvider damageProvider;
    public int InitialAttack;
    public int InitialDurability;
    
    public bool IsFriendly (string morphidGuid) {
        return morphidGuid == MorphidGUID;
    }
    
    public bool IsEnemy (string morphidGuid) {
        return morphidGuid != MorphidGUID;
    }
    
    public static bool IsDead (Minion minion) {
        return minion == null || minion.Durability <= 0;
    }

    public bool AffectedByTerrain(TerrainType terrainType) {
        Lane lane = GameState.GetLane(this);
        if (lane == null) {
            return false;
        }
        return lane.isTerrainType(terrainType) && !Hazmat;
    }
    
    public int Attack {
        get {
            if (Network.peerType == NetworkPeerType.Client) {
                return CurrentAttack;
            }
            return damageProvider.Provider();
        }
    }

    public int Durability {
        get {
            if (Network.peerType == NetworkPeerType.Client) {
                return CurrentDurability;
            }
            return DurabilityProvider.GetDurability(this);
        }
    }
    
    public Minion () {
        GUID = Guid.NewGuid().ToString();
        damageProvider = new MinionDamageProvider(this);
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

        GameStateWatcher.OnAttack(GUID);

        Lane myLane = GameState.GetLane(this);
        int laneIndex = GameState.GetLaneIndex(myLane);
        Minion defender = GameState.GetLaneDefender(laneIndex);
        if (defender != null) {
            damageProvider.Target = defender.GUID;

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
                Health = defender.Durability
            }
            );
            AnimationSignalManager.SendRPC(
                AnimationSignalManager.Singleton.QueueMinionAliveLie,
                new MinionAliveLie() {
                GUID = defender.GUID,
                Alive = true
            }
            );

            damageProvider.Apply(defender.GUID);
        }
        else {
            if (!Defensive) {
                damageProvider.Apply(GameState.InactiveMorphid.GUID);
            }
        }
    }

    public void OnTurnBegin () {
        if (OnFire) {
            GameState.DamageGuid(new Damage() {
                Target = GUID,
                Magnitude = 1
            });
        }

        bool attack = MorphidGUID == GameState.ActiveMorphid.GUID;
        if (attack) {
            DoAttack();
            if (AffectedByTerrain(TerrainType.Acid)) {
                GameState.DamageGuid(new Damage() {
                    Target = GUID,
                    Magnitude = 2
                });
            }
        }
    }

    public void OnSpawn () {
        if (Blitz || AffectedByTerrain(TerrainType.Slick)) {
            DoAttack();
        }
    }

    public void Template() {
        TemplateStatus.Templating = true;

        CurrentAttack = Attack;
        CurrentDurability = Durability;

        TemplateStatus.Templating = false;
    }
}