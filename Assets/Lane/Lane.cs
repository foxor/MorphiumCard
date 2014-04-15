using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Lane {
    
    [SerializeField]
    [ProtoMember(1)]
    public string GUID;

    [SerializeField]
    [ProtoMember(2)]
    public Minion[] Minions;

    [SerializeField]
    [ProtoMember(3)]
    public int TerrainType;
    
    static Lane () {
        RuntimeTypeModel.Default.Add(typeof(Lane), true)[2].SupportNull = true;
    }
    
    public Lane () {
        GUID = Guid.NewGuid().ToString();
        Minions = new Minion[0];
    }
    
    public Minion FriendlyMinion (string morphidGuid) {
        for (int i = 0; i < Minions.Length; i++) {
            if (!Minion.IsDead(Minions[i]) && Minions[i].IsFriendly(morphidGuid)) {
                return Minions[i];
            }
        }
        return null;
    }
    
    public Minion EnemyMinion (string morphidGuid) {
        for (int i = 0; i < Minions.Length; i++) {
            if (!Minion.IsDead(Minions[i]) && Minions[i].IsEnemy(morphidGuid)) {
                return Minions[i];
            }
        }
        return null;
    }

    public bool isEmpty(string morphidGuid)
    {
        return Minion.IsDead(FriendlyMinion(GameState.ActiveMorphid.GUID));
    }

    public bool isTerrainType(TerrainType terrainType) {
        if (Network.peerType == NetworkPeerType.Server) {
            terrainType = TerrainProvider.getTerrainType(GUID);
        }
        return TerrainType == (int)terrainType;
    }
}