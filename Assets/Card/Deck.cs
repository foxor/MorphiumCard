﻿using UnityEngine;
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
			if (i < 6) {
				yield return new Card() {
					Cost = 5,
					Reflect = new Reflect(){Magnitude = 2},
					Durability = 1,
					Name = "Ambush",
					Text = "Reflects the next 2 damage"
				};
			}
			else {
				yield return new Card() {
					Cost = 4,
					Healing = new Healing(){Magnitude = 2},
					Durability = 2,
					Name = "Repair Bot",
					Text = "Repairs 2 health"
				};
			}
		}
	}
	
	protected IEnumerable<Card> ChestCards() {
		for (int i = 0; i < MAX_CARDS; i++) {
			int cost = Mathf.CeilToInt((((float)i) + 1f) / 2f);
			int totalPower = cost * 2;
			int attack = UnityEngine.Random.Range(0, totalPower - 1);
			int defense = totalPower - attack;
			yield return new Card() {
				Cost = cost,
				Spawn = new Spawn() {
					Minion = new Minion() {
						Attack = attack,
						Defense = defense
					}
				},
				Targeting = (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Lane,
				Durability = 1,
				Name = "Grizzly Bear",
				Text = attack + "/" + defense + " Bear"
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
			Cards = HeadCards().OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToArray();
			break;
		case Slot.Chest:
			Cards = ChestCards().OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToArray();
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