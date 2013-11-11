using CsvHelper;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CardData {
	public string Name;
	public string Text;
	public int Cost;
	public bool TargetOne;
	public bool TargetAll;
	public int Targeting;
	public int Head;
	public int Chest;
	public int Arm;
	public int Leg;
	public int Damage;
	public int Healing;
	public int Reflect;
	public int Engine;
	public int Attack;
	public int Defense;
}

public class Importer {
	public const string CARD_FILE_NAME = "Cards";
	
	protected Card[] cards;
	protected Card[] Cards {
		get {
			if (cards == null) {
				cards = ReadCards().ToArray();
			}
			return cards;
		}
	}
	
	public IEnumerable<Card> CardsBySlot(Slot slot) {
		ReadCards();
		foreach (Card c in Cards) {
			if (c.Slot == slot) {
				for (int i = 0; i < c.Appearances; i++) {
					yield return c;
				}
			}
		}
	}
	
	protected IEnumerable<CardData> ReadData() {
		TextAsset CardFile = (TextAsset)Resources.Load(CARD_FILE_NAME);
		MemoryStream memoryStream = new MemoryStream(CardFile.bytes);
		StreamReader stream = new StreamReader(memoryStream);
		CsvReader reader = new CsvReader(stream);
		while (reader.Read()) {
			CardData data = new CardData();
			data.Name = reader.GetField<string>("Name");
			data.Text = reader.GetField<string>("Text");
			data.Cost = reader.GetField<int>("Cost");
			data.TargetOne = reader.GetField<bool>("TargetOne");
			data.TargetAll = reader.GetField<bool>("TargetAll");
			data.Targeting = reader.GetField<int>("Targeting");
			data.Head = reader.GetField<int>("Head");
			data.Chest = reader.GetField<int>("Chest");
			data.Arm = reader.GetField<int>("Arm");
			data.Leg = reader.GetField<int>("Leg");
			data.Damage = reader.GetField<int>("Damage");
			data.Healing = reader.GetField<int>("Healing");
			data.Reflect = reader.GetField<int>("Reflect");
			data.Engine = reader.GetField<int>("Engine");
			data.Attack = reader.GetField<int>("Attack");
			data.Defense = reader.GetField<int>("Defense");
			yield return data;
		}
	}
	
	protected IEnumerable<Card> ReadCards() {
		foreach (CardData data in ReadData()) {
			Slot slot = data.Arm > 0 ? Slot.Arm : (
				data.Chest > 0 ? Slot.Chest : (
				data.Leg > 0 ? Slot.Legs : Slot.Head
			));
			Card c = new Card();
			c.Appearances = slot == Slot.Arm ? data.Arm : (
				slot == Slot.Chest ? data.Chest : (
				slot == Slot.Head ? data.Head : data.Leg
			));
			c.Cost = data.Cost;
			c.Damage = new Damage(){Magnitude = data.Damage};
			c.Engine = new Engine(){Magnitude = data.Engine};
			c.Healing = new Healing(){Magnitude = data.Healing};
			c.Name = data.Name;
			c.Reflect = new Reflect(){Magnitude = data.Reflect};
			c.Slot = slot;
			c.Spawn = new Spawn(){Minion = new Minion(){Attack = data.Attack, Defense = data.Defense}};
			c.TargetingType = data.TargetAll ? TargetingType.All : TargetingType.Single;
			c.Targeting = data.Targeting;
			c.Text = data.Text;
			yield return c;
		}
	}
}