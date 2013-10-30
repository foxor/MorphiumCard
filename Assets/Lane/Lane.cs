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
	public Minion[] Minions;
	
	[SerializeField]
	[ProtoMember(2)]
	public string GUID;
	
	public Lane() {
		Minions = new Minion[2];
	}
	
	public Minion FriendlyMinion(string morphidGuid) {
		for (int i = 0; i < GameState.Singleton.Morphids.Length; i++) {
			if (GameState.Singleton.Morphids[i].GUID == morphidGuid) {
				return Minions[i];
			}
		}
		return null;
	}
	
	public Minion EnemyMinion(string morphidGuid) {
		for (int i = 0; i < GameState.Singleton.Morphids.Length; i++) {
			if (GameState.Singleton.Morphids[i].GUID != morphidGuid) {
				return Minions[i];
			}
		}
		return null;
	}
	
	public void SpawnFriendly(Minion minion) {
		for (int i = 0; i < GameState.Singleton.Morphids.Length; i++) {
			if (GameState.Singleton.Morphids[i].GUID == GameState.ActiveMorphid.GUID) {
				Minions[i] = minion.SerializeProtoBytes().DeserializeProtoBytes<Minion>();
			}
		}
	}
}