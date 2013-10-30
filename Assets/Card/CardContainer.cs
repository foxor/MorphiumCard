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
	[NonSerialized] //Prevents unity from copying the guids around
	[ProtoMember(1)]
	public string GUID;
	
	[SerializeField]
	[ProtoMember(2)]
	public String Name;
	
	[SerializeField]
	[ProtoMember(3)]
	public String Text;
	
	[SerializeField]
	[ProtoMember(4)]
	public int Cost;
	
	[SerializeField]
	[ProtoMember(5)]
	public int Durability;
	
	[SerializeField]
	[ProtoMember(6)]
	public Damage Damage;
	
	[SerializeField]
	[ProtoMember(7)]
	public Healing Healing;
	
	[SerializeField]
	[ProtoMember(8)]
	public Reflect Reflect;
	
	[SerializeField]
	[ProtoMember(9)]
	public EngineBurn EngineBurn;
	
	[SerializeField]
	[ProtoMember(10)]
	public EngineRamp EngineRamp;
	
	public Card() {
		GUID = Guid.NewGuid().ToString();
		Damage = new Damage();
		Healing = new Healing();
		Reflect = new Reflect();
		EngineBurn = new EngineBurn();
		EngineRamp = new EngineRamp();
	}
	
	public void Process(string targetGuid) {
		Morphid self = GameState.GetMorphid(targetGuid);
		if (self.Morphium >= Cost) {
			self.Morphium -= Cost;
			Damage.Apply(targetGuid);
			Healing.Apply(targetGuid);
			Reflect.Apply(targetGuid);
			EngineBurn.Apply(targetGuid);
			EngineRamp.Apply(targetGuid);
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
	
	public void Draw() {
		for (int i = 0; i < Decks.Count(); i++) {
			Cards[i] = Decks[i].Draw();
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