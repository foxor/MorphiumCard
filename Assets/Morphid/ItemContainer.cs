using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;
	
[Serializable]
[ProtoContract]
public class Item {
	// TODO
}

[Serializable]
[ProtoContract]
public class ItemContainer {
	
	[SerializeField]
	[ProtoMember(1)]
	public Item[] Items;
}