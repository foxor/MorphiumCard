using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

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
	public int Cost;
	
	[SerializeField]
	[ProtoMember(4)]
	public Damage Damage;
	
	[NonSerialized] //Prevents unity from copying the guids around
	[ProtoMember(5)]
	public string GUID;
	
	public Card() {
		GUID = Guid.NewGuid().ToString();
	}
	
	public void Process(string friendlyGuid) {
		Morphid self = Server.GetMorphid(friendlyGuid);
		if (self.Morphium >= Cost) {
			self.Morphium -= Cost;
			Damage.Apply(friendlyGuid);
		}
	}
}

[Serializable]
[ProtoContract]
public class CardContainer {
	[SerializeField]
	[ProtoMember(1)]
	public Card[] Cards;
	
	public Card FromGuid(string guid) {
		return Cards.Where(x => x.GUID == guid).Single();
	}
}