using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Deck {
	public const int MAX_CARDS = 15;
	
	[SerializeField]
	[ProtoMember(1)]
	public Card[] Cards;
	
	[SerializeField]
	[ProtoMember(2)]
	public int Index;
	
	[SerializeField]
	[ProtoMember(3)]
	public Slot Slot;
	
	protected IEnumerable<Card> ArmCards() {
		for (int i = 0; i < MAX_CARDS; i++) {
			int Damage = Mathf.CeilToInt(((float)MAX_CARDS) / 5f);
			yield return new Card() {
				Cost = Damage + 1,
				Damage = new Damage(){Magnitude = Damage},
				Durability = 4 - Damage,
				Name = "Fireball",
				Text = "Deals " + Damage + " damage"
			};
		}
	}
	
	public void Shuffle() {
		switch (Slot) {
		case Slot.Head:
			Cards = new Card[0];
			break;
		case Slot.Chest:
			Cards = new Card[0];
			break;
		case Slot.Arm:
			Cards = ArmCards().OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToArray();
			break;
		case Slot.Legs:
			Cards = new Card[0];
			break;
		}
	}
	
	public Card Draw() {
		if (Index <= Cards.Count() - 1) {
			return Cards[Index++];
		}
		return null;
	}
}