﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameState : MonoBehaviour {
    protected const int NUM_PLAYERS = 2;
    protected const int NUM_LANES = 3;
    public static GameState Singleton;
    public int PlayerCount;
    public Morphid[] Morphids;
    public Lane[] Lanes;
    public int ActivePlayer;
    
    public void Awake () {
        Singleton = this;
        if (Network.peerType == NetworkPeerType.Client) {
            StartCoroutine(SetupCoroutine());
        }
        Morphids = new Morphid[NUM_PLAYERS].Select(x => new Morphid ().Setup()).ToArray();
        Lanes = new Lane[NUM_LANES].Select(x => new Lane ()).ToArray();
    }
    
    public void AddMorphid (string guid, DeckList deckInfo) {
        Morphids[PlayerCount].GUID = guid;
        Morphids[PlayerCount].CardContainer.Setup(deckInfo);
        Morphids[PlayerCount].AttachmentContainer.Setup();
        
        if (++PlayerCount >= NUM_PLAYERS) {
            ActivePlayer = UnityEngine.Random.Range(0, 1);
        }
    }
    
    protected IEnumerator SetupCoroutine () {
        while (PlayerCount < NUM_PLAYERS) {
            yield return 0;
        }
    }

    protected void TemplateMinions() {
        if (Network.peerType != NetworkPeerType.Server || GameState.ActiveMorphid == null) {
            return;
        }
        for (int i = 0; i < Lanes.Length; i++) {
            Minion Friendly = Lanes[i].FriendlyMinion(GameState.ActiveMorphid.GUID);
            Minion Enemy = Lanes[i].EnemyMinion(GameState.ActiveMorphid.GUID);
            
            if (Friendly != null) {
                Friendly.Template();
            }
            if (Enemy != null) {
                Enemy.Template();
            }
        }
    }

    protected void SyncMinionSprites () {
        if (Network.peerType == NetworkPeerType.Server || Morphid.LocalPlayer == null) {
            return;
        }
        for (int i = 0; i < Lanes.Length; i++) {
            Minion Friendly = Lanes[i].FriendlyMinion(Morphid.LocalPlayer.GUID);
            Minion Enemy = Lanes[i].EnemyMinion(Morphid.LocalPlayer.GUID);
            
            if (Friendly != null) {
                Friendly.GameObject = UI.Singleton.Target.FriendlyMinions[i].Sprite;
            }
            if (Enemy != null) {
                Enemy.GameObject = UI.Singleton.Target.EnemyMinions[i].Sprite;
            }
        }
    }
    
    public void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
        stream.SerializeProto<Morphid[]>(ref Morphids);
        TemplateMinions();
        stream.SerializeProto<Lane[]>(ref Lanes);
        SyncMinionSprites();
        stream.SerializeProto<int>(ref ActivePlayer);
        stream.SerializeProto<int>(ref PlayerCount);
    }
    
    public static bool IsLocalActive {
        get {
            return Singleton == null || (Singleton.PlayerCount >= NUM_PLAYERS &&
                ActiveMorphid.GUID == Client.GUID);
        }
    }
    
    public void SwapTurn () {
        ActivePlayer = (ActivePlayer + 1) % NUM_PLAYERS;
    }
    
    public static Morphid GetMorphid (string guid) {
        return Singleton == null || guid == null ? null : Singleton.Morphids.Where(x => x.GUID == guid).SingleOrDefault();
    }
    
    public static Morphid GetEnemy (string guid) {
        return Singleton == null || guid == null ? null : Singleton.Morphids.Where(x => x.GUID != guid).SingleOrDefault();
    }
    
    public static Lane GetLane (string guid) {
        return Singleton == null || guid == null ? null : Singleton.Lanes.Where(x => x.GUID == guid).SingleOrDefault();
    }
    
    public static Lane GetLane (int lane) {
        return Singleton == null ? null : Singleton.Lanes[lane];
    }
    
    public static Lane GetLane (Minion minion) {
        return Singleton == null ? null : Singleton.Lanes.Where(x => x.Minions.Any(y => y.GUID == minion.GUID)).SingleOrDefault();
    }

    public static int GetLaneIndex (Lane lane) {
        for (int index = 0; index < Singleton.Lanes.Length; index++) {
            if (Singleton.Lanes[index].GUID == lane.GUID) {
                return index;
            }
        }
        throw new ArgumentException("No such lane exists");
    }
    
    public static Minion GetMinion (string guid) {
        return Singleton == null || guid == null ? null : Singleton.Lanes.SelectMany(x => x.Minions).Where(x => x != null && x.GUID == guid).SingleOrDefault();
    }
    
    public static IEnumerable<Minion> GetMinions () {
        return Singleton == null ? null : Singleton.Lanes.SelectMany(x => x.Minions).Where(x => x != null);
    }
    
    public static Morphid ActiveMorphid {
        get {
            return Singleton.Morphids[Singleton.ActivePlayer];
        }
    }
    
    public static Morphid InactiveMorphid {
        get {
            return Singleton.Morphids[(Singleton.ActivePlayer + 1) % 2];
        }
    }
    
    public static Minion GetLaneDefender(int lane)
    {
        Minion center = GetLane(lane).EnemyMinion(ActiveMorphid.GUID);
        Minion left = lane > 0 ? GetLane(lane - 1).EnemyMinion(ActiveMorphid.GUID)  : null;
        Minion right = lane < 2 ? GetLane(lane + 1).EnemyMinion(ActiveMorphid.GUID) : null;
        
        if (center != null && center.Protect)
        {
            return center;
        }
        else if (left != null && left.Protect)
        {
            return left;
        }
        else if (right != null && right.Protect)
        {
            return right;
        }
        return center;
    }
    
    public static void DestroyMinion (string guid) {
        Minion minion = GetMinion(guid);
        if (minion != null) {
            minion.OnDeath();
            GameStateWatcher.OnMinionDeath(minion);
        }

        Lane owner = Singleton.Lanes.Where(x => x.Minions.Any(y => y.GUID == guid)).SingleOrDefault();
        if (owner != null) {
            owner.Minions = owner.Minions.Where(x => x.GUID != guid).ToArray();
        }
    }
    
    public static void DamageGuid (Damage damage) {
        DamagePrevention.PreventDamage(damage);
        if (damage.Magnitude <= 0) {
            return;
        }

        Morphid morphid = GetMorphid(damage.Target);
        Minion minion = GetMinion(damage.Target);
        if (morphid != null) {
            morphid.Health -= damage.Magnitude;
            morphid.AttachmentContainer.Damage(damage.Magnitude);
            GameStateWatcher.OnDamage(damage);
        }
        if (minion != null) {
            minion.InitialDurability -= damage.Magnitude;
            if (Minion.IsDead(minion)) {
                DestroyMinion(minion.GUID);
            }
            GameStateWatcher.OnDamage(damage);
        }
    }
    
    public static void LaneDamage (LaneDamage damage) {
        Lane lane = GetLane(damage.Target);
        if (lane != null) {
            Minion minion = lane.FriendlyMinion(damage.EnemyMorphid);
            if (minion != null) {
                int minionDamage = Mathf.Min(damage.Magnitude, minion.Durability);
                DamageGuid(new Damage() {
                    Target = minion.GUID,
                    Source = damage.Source,
                    Magnitude = minionDamage
                });
                damage.Magnitude -= minionDamage;
            }
        }
        if (damage.Magnitude > 0) {
            DamageGuid(new Damage() {
                Target = damage.EnemyMorphid,
                Source = damage.Source,
                Magnitude = damage.Magnitude
            });
        }
    }

    public static void RepairGuid(string guid, int healing)
    {
        Morphid morphid = GetMorphid(guid);
        Minion minion = GetMinion(guid);
        if (morphid != null)
        {
            morphid.Health += healing;
        }
        if (minion != null)
        {
            minion.InitialDurability += healing;
        }
    }
    
    public static void AddMorphium(string guid, int morphium)
    {
        Morphid morphid = GetMorphid(guid);
        if (morphid != null)
        {
            morphid.Morphium = Math.Min(morphid.Morphium + morphium, Morphid.MAX_MORPHIUM);
        }
    }

    public static void AddWeight(string guid, int weight)
    {
        Morphid morphid = GetMorphid(guid);
        if (morphid != null)
        {
            morphid.Weight += weight;
        }
    }

	public static void AddParts(string guid, int parts)
	{
		Morphid morphid = GetMorphid(guid);
		if (morphid != null)
		{
			morphid.Parts += parts;
		}
	}

    public static void ConsumeParts(string guid)
    {
        Morphid morphid = GetMorphid(guid);
        if (morphid != null)
        {
            morphid.Parts = 0;
        }
    }

    public static void AddEngine(string guid, int engine)
    {
        Morphid morphid = GetMorphid(guid);
        if (morphid != null)
        {
            morphid.Engine = Math.Max(0, Math.Min(morphid.Engine + engine, Morphid.MAX_ENGINE));
        }
    }

    public static Minion SummonMinion(string laneGuid, int attack, int defense, MinionBuilder builder)
    {
        Lane lane = GameState.GetLane(laneGuid);
        if (lane != null)
        {
            if (builder == null) {
                builder = new MinionBuilder();
            }

            Minion minion = new Minion()
            {
                InitialAttack = attack,
                InitialDurability = defense,

                Defensive = builder.Defensive,
                Protect = builder.Protect,
                Scrounge = builder.Scrounge,
                OnFire = builder.OnFire,
                Blitz = builder.Blitz,
                Hazmat = builder.Hazmat,

                GUID = Guid.NewGuid().ToString(),
                MorphidGUID = GameState.ActiveMorphid.GUID
            };

            lane.Minions = lane.Minions.
                Where(x => x.MorphidGUID != minion.MorphidGUID).
                Concat(new Minion[1] {minion}).
                ToArray();

            GameStateWatcher.OnMinionSpawn(minion);
            minion.OnSpawn();

            return minion;
        }
        return null;
    }

    public static void SetResearch(string guid, Action research)
    {
        Morphid m = GameState.GetMorphid(guid);
        if (m != null)
        {
            m.Research = research;
        }
    }
    
    public static void AddEngineSequence(string guid, Action sequence)
    {
        Morphid m = GameState.GetMorphid(guid);
        if (m != null)
        {
            m.EngineSequence += sequence;
        }
    }
    
    public static void RemoveEngineSequence(string guid, Action sequence)
    {
        Morphid m = GameState.GetMorphid(guid);
        if (m != null)
        {
            m.EngineSequence -= sequence;
        }
    }

    public static void BuffMinion(string guid, int attackBuff, int defenseBuff) {
        Minion minion = GetMinion(guid);
        if (minion != null) {
            minion.InitialAttack = Mathf.Max(0, minion.InitialAttack + attackBuff);
            minion.InitialDurability += defenseBuff;
            if (minion.Durability <= 0) {
                DestroyMinion(guid);
            }
        }
    }

    public static void Attach(string morphidGuid, Attachment attachment, Slot slot) {
        Morphid m = GameState.GetMorphid(morphidGuid);
        if (m != null)
        {
            m.AttachmentContainer.Attach(slot, attachment);
        }
    }
    
    public static void FireSetGuid(string guid, bool onFire = true)
    {
        Morphid morphid = GetMorphid(guid);
        Minion minion = GetMinion(guid);
        if (morphid != null)
        {
            morphid.OnFire = onFire;
        }
        if (minion != null)
        {
            minion.OnFire = onFire;
        }
    }

    public static void ChargeSet(string morphidGuid, Slot toAlter, bool newCharged) {
        Morphid morphid = GetMorphid(morphidGuid);
        if (morphid != null) {
            morphid.CardContainer.ComboSlot(toAlter, newCharged);
        }
    }

    public static void TerrainChange(string laneGuid, TerrainType terrainType) {
        Lane lane = GetLane(laneGuid);
        if (lane != null) {
            lane.TerrainType = (int)terrainType;
        }
    }

    public static void Reload(string morphidGuid, Slot slot, string targetGuid) {
        Morphid morphid = GetMorphid(morphidGuid);
        if (morphid != null) {
            Card comboCard = morphid.CardContainer.GetCard(slot);
            if (comboCard != null) {
                TargetingRequirements req = new TargetingRequirements(comboCard.Effect);
                if (comboCard.Cost > morphid.Morphium || !req.AllowedTargets().Contains(targetGuid)) {
                    return;
                }
                morphid.PlayCard(comboCard.GUID, targetGuid);
            }
        }
    }

    public static void ReduceAttack(string minionGuid, int redution) {
        Minion minion = GetMinion(minionGuid);
        if (minion != null) {
            minion.InitialAttack -= redution;
        }
    }
}