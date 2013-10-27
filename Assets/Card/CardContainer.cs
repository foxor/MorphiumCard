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
	public int Durability;
	
	[SerializeField]
	[ProtoMember(5)]
	public Damage Damage;
	
	[NonSerialized] //Prevents unity from copying the guids around
	[ProtoMember(6)]
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
		Durability -= 1;
	}
}

[Serializable]
[ProtoContract]
public class CardContainer {
	[SerializeField]
	[ProtoMember(1)]
	public Card[] Cards;
	
	[SerializeField]
	[ProtoMember(2)]
	public Deck[] Decks;
	
	static CardContainer() {
		RuntimeTypeModel.Default.Add(typeof(CardContainer), true)[1].SupportNull = true;
	}
	
	public Card FromGuid(string guid) {
		return Cards.Where(x => x != null && x.GUID == guid).Single();
	}
	
	public void RemoveBroken() {
		for (int i = 0; i < Cards.Length; i++) {
			if (Cards[i] != null && Cards[i].Durability <= 0) {
				Cards[i] = null;
			}
		}
	}
	
	public void DrawFromDeck(int deck) {
		Cards[deck] = Decks[deck].Draw();
	}
	
	public void Setup() {
		Slot[] Slots = Enum.GetValues(typeof(Slot)).Cast<Slot>().OrderBy(x => x.Order()).ToArray();
		Cards = new Card[Slots.Count()];
		Decks = new Deck[Slots.Count()];
		for (int i = 0; i < Slots.Count(); i++) {
			Decks[i] = new Deck(){
				Slot = Slots[i]
			};
			Decks[i].Shuffle();
			Cards[i] = Decks[i].Draw();
		}
	}
}