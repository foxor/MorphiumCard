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
	public string[] OwnerGuids;
	
	public Minion FriendlyMinion(string morphidGuid) {
		for (int i = 0; i < OwnerGuids.Length; i++) {
			if (OwnerGuids[i] == morphidGuid) {
				return Minions[i];
			}
		}
		return null;
	}
	
	public Minion EnemyMinion(string morphidGuid) {
		for (int i = 0; i < OwnerGuids.Length; i++) {
			if (OwnerGuids[i] != morphidGuid) {
				return Minions[i];
			}
		}
		return null;
	}
}