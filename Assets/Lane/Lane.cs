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
	
	static Lane () {
		RuntimeTypeModel.Default.Add(typeof(Lane), true)[2].SupportNull = true;
	}
	
	public Lane() {
		GUID = Guid.NewGuid().ToString();
		Minions = new Minion[0];
	}
	
	public Minion FriendlyMinion(string morphidGuid) {
		for (int i = 0; i < Minions.Length; i++) {
			if (!Minion.IsDead(Minions[i]) && Minions[i].IsFriendly(morphidGuid)) {
				return Minions[i];
			}
		}
		return null;
	}
	
	public Minion EnemyMinion(string morphidGuid) {
		for (int i = 0; i < Minions.Length; i++) {
			if (!Minion.IsDead(Minions[i]) && Minions[i].IsEnemy(morphidGuid)) {
				return Minions[i];
			}
		}
		return null;
	}
	
	public void SpawnFriendly(Minion minion) {
		Minion toAdd = minion.SerializeProtoBytes().DeserializeProtoBytes<Minion>();
		toAdd.MorphidGUID = GameState.ActiveMorphid.GUID;
		Minions = Minions.
			Where(x => !Minion.IsDead(x) && x.GUID != GameState.ActiveMorphid.GUID).
			Concat(new Minion[1] {toAdd}).
			ToArray();
	}
}