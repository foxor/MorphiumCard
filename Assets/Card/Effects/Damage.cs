using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Damage : Effect {
	[SerializeField]
	[ProtoMember(1)]
	public int Magnitude;
	
	public override void Apply (string friendlyGuid) {
		Morphid enemy = GameState.GetEnemy(friendlyGuid);
		int damage = Magnitude;
		if (enemy.Reflect > 0) {
			int reflected = Mathf.Min(enemy.Reflect, damage);
			enemy.Reflect -= reflected;
			GameState.GetMorphid(friendlyGuid).Health -= reflected;
			damage -= reflected;
		}
		enemy.Health -= damage;
	}
}
