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
	
	protected IEnumerable<Card> HeadCards() {
		for (int i = 0; i < MAX_CARDS; i++) {
			yield return new Card() {
				Cost = 5,
				Reflect = new Reflect(){Magnitude = 2},
				Durability = 1,
				Name = "Ambush",
				Text = "Reflects the next 2 damage"
			};
		}
	}
	
	protected IEnumerable<Card> ChestCards() {
		for (int i = 0; i < MAX_CARDS; i++) {
			yield return new Card() {
				Cost = 4,
				Healing = new Healing(){Magnitude = 2},
				Durability = 2,
				Name = "Repair Bot",
				Text = "Repairs 2 health"
			};
		}
	}
	
	protected IEnumerable<Card> ArmCards() {
		for (int i = 0; i < MAX_CARDS; i++) {
			int Damage = Mathf.CeilToInt(((float)(i + 1)) / 2f);
			yield return new Card() {
				Cost = Damage + 1,
				Damage = new Damage(){Magnitude = Damage},
				Durability = 2,
				Name = "Fireball",
				Text = "Deals " + Damage + " damage"
			};
		}
	}
	
	public void Shuffle() {
		switch (Slot) {
		case Slot.Head:
			Cards = HeadCards().ToArray();
			break;
		case Slot.Chest:
			Cards = ChestCards().ToArray();
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