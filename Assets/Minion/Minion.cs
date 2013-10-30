using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Minion {
	
	[SerializeField]
	[ProtoMember(1)]
	public int Attack;
	
	[SerializeField]
	[ProtoMember(2)]
	public int Defense;
	
	[SerializeField]
	[ProtoMember(3)]
	public string GUID;
}