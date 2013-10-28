using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class EngineBurn : Effect {
	[SerializeField]
	[ProtoMember(1)]
	public int Magnitude;
	
	public override void Apply (string friendlyGuid) {
		Morphid enemy = Server.GetEnemy(friendlyGuid);
		enemy.Engine -= Magnitude;
	}
}