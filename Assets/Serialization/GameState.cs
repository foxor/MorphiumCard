using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GameState : MonoBehaviour {
    protected const int NUM_PLAYERS = 2;
    protected const int NUM_LANES = 3;
    public static GameState Singleton;
    public int PlayerCount;
    public Morphid[] Morphids;
    public Lane[] Lanes;
    protected int ActivePlayer;
    
    public void Awake () {
        Singleton = this;
        if (Network.peerType == NetworkPeerType.Client) {
            StartCoroutine(SetupCoroutine());
        }
        Morphids = new Morphid[NUM_PLAYERS].Select(x => new Morphid ().Setup()).ToArray();
        Lanes = new Lane[NUM_LANES].Select(x => new Lane ()).ToArray();
    }
    
    public void AddMorphid (string guid) {
        Morphids[PlayerCount].GUID = guid;
        Morphids[PlayerCount].CardContainer.Setup();
        
        if (++PlayerCount >= NUM_PLAYERS) {
            ActivePlayer = UnityEngine.Random.Range(0, 1);
        }
    }
    
    protected IEnumerator SetupCoroutine () {
        while (PlayerCount < NUM_PLAYERS) {
            yield return 0;
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
        stream.SerializeProto<Lane[]>(ref Lanes);
        SyncMinionSprites();
        stream.SerializeProto<int>(ref ActivePlayer);
        stream.SerializeProto<int>(ref PlayerCount);
    }
    
    public static bool IsLocalActive {
        get {
            return Singleton.PlayerCount >= NUM_PLAYERS &&
                ActiveMorphid.GUID == Client.GUID;
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
    
    public static Minion GetMinion (string guid) {
        return Singleton == null || guid == null ? null : Singleton.Lanes.SelectMany(x => x.Minions).Where(x => x != null && x.GUID == guid).SingleOrDefault();
    }
    
    public static Morphid ActiveMorphid {
        get {
            return Singleton.Morphids[Singleton.ActivePlayer];
        }
    }
    
    public static void DestroyMinion (string guid) {
        Lane owner = Singleton.Lanes.Where(x => x.Minions.Any(y => y.GUID == guid)).SingleOrDefault();
        if (owner != null) {
            owner.Minions = owner.Minions.Where(x => x.GUID != guid).ToArray();
        }
    }
    
    public static void DamageGuid (string guid, int damage) {
        Morphid morphid = GetMorphid(guid);
        Minion minion = GetMinion(guid);
        if (morphid != null) {
            morphid.Health -= damage;
        }
        if (minion != null) {
            minion.Defense -= damage;
            if (minion.Defense <= 0) {
                DestroyMinion(guid);
            }
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
            minion.Defense += healing;
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

    public static void AddEngine(string guid, int engine)
    {
        Morphid morphid = GetMorphid(guid);
        if (morphid != null)
        {
            morphid.Engine += engine;
        }
    }

    public static Minion SummonMinion(string laneGuid, int attack, int defense, bool defensive)
    {
        Lane lane = GameState.GetLane(laneGuid);
        if (lane != null)
        {
            Minion m = new Minion()
            {
                Attack = attack,
                Defense = defense,
                Defensive = defensive
            };
            lane.SpawnFriendly(m);
            return m;
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
            if (m.EngineSequence == null)
            {
                m.EngineSequence = sequence;
            }
            else
            {
                Action oldSequence = m.EngineSequence;
                m.EngineSequence = () =>
                {
                    oldSequence();
                    sequence();
                };
            }
        }
    }
}