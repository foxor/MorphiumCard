using CsvHelper;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CardData {
    public string Name;
    public string Text;
    public string Manufacturer;
    public int Cost;
    public string Slot;
    public string Effect;
    public string Arguments;
    public string Target;
    public string Targeted;
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
    
    public IEnumerable<Card> CardsBySlot (Slot slot) {
        ReadCards();
        foreach (Card c in Cards) {
            if (c.Slot == slot) {
                for (int i = 0; i < c.Appearances; i++) {
                    yield return c.Copy();
                }
            }
        }
    }
    
    protected IEnumerable<CardData> ReadData () {
        TextAsset CardFile = (TextAsset)Resources.Load(CARD_FILE_NAME);
        MemoryStream memoryStream = new MemoryStream (CardFile.bytes);
        StreamReader stream = new StreamReader (memoryStream);
        CsvReader reader = new CsvReader (stream);
        while (reader.Read()) {
            CardData data = new CardData ();
            data.Name = reader.GetField<string>("Name");
            data.Text = reader.GetField<string>("Text");
            data.Manufacturer = reader.GetField<string>("Manufacturer");
            data.Cost = reader.GetField<int>("Cost");
            data.Slot = reader.GetField<string>("Slot");
            data.Effect = reader.GetField<string>("Effect");
            data.Arguments = reader.GetField<string>("Arguments");
            data.Target = reader.GetField<string>("Target");
            data.Targeted = reader.GetField<string>("Targeted");
            yield return data;
        }
    }
    
    protected IEnumerable<Card> ReadCards () {
        foreach (CardData data in ReadData()) {
            Slot slot = (Slot)Enum.Parse(typeof(Slot), data.Slot);
            Card c = new Card ();
            string[] Effects = data.Effect.Split(',');
            string[] Arguments = data.Arguments.Split(',');
            int[] Targets = data.Target.Split(',').Select(x => int.Parse(x)).ToArray();
            TargetingType[] Targeted = data.Targeted.Split(',').Select(x => bool.Parse(x) ? TargetingType.Single : TargetingType.All).ToArray();
            c.Build(Effects, Arguments, Targets, Targeted);
            c.Appearances = 1;
            c.Cost = data.Cost;
            c.Manufacturer = data.Manufacturer;
            c.Name = data.Name;
            c.Slot = slot;
            c.Text = data.Text;
            yield return c;
        }
    }
}