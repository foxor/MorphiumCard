﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
public enum CardType {
	Attack,
	Utility
}

[Serializable]
[ProtoContract]
public class Card {
	[SerializeField]
	[ProtoMember(1)]
	public String Name;
	
	[SerializeField]
	[ProtoMember(2)]
	public String Text;
	
	[SerializeField]
	[ProtoMember(3)]
	public CardType Type;
}

[Serializable]
[ProtoContract]
public class CardContainer {
	[SerializeField]
	[ProtoMember(1)]
	public Card[] Cards;
}