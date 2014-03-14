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
    public string Cost;
    public string Slot;
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
                for (int i = 0; i < 1; i++) {
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
            data.Cost = reader.GetField<string>("Cost");
            data.Slot = reader.GetField<string>("Slot");
            yield return data;
        }
    }
    
    protected IEnumerable<Card> ReadCards () {
        foreach (CardData data in ReadData()) {
            Slot slot = (Slot)Enum.Parse(typeof(Slot), data.Slot);
            Card c = new Card ();
            c.Name = data.Name;
            c.Manufacturer = data.Manufacturer;
            c.Slot = slot;

            c.Effect = CardEffectTable.Map(data.Name, data.Text);
            if (c.Effect != null) {
                c.Template();
                yield return c;
            }
        }
    }
}