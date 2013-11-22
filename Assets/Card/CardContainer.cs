using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

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
	
	public void ReplaceCard(Card c) {
		for (int i = 0; i < Cards.Length; i++) {
			if (Cards[i] == c) {
                Cards[i] = Decks[i].Draw();
			}
		}
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