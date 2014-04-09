using CsvHelper;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[Serializable]
public class CardData {
    public string Name;
    public string Text;
    public string Manufacturer;
    public string Cost;
    public string Slot;
    public string Notes;
}

public class Importer : MonoBehaviour {
    public const string CARD_FILE_NAME = "Cards";

    public static Importer Singleton;

    protected Card[] cards;

    public Card[] Cards {
        get {
            if (cards == null) {
                cards = ReadCards().ToArray();
            }
            return cards;
        }
    }

    [SerializeField]
    public List<CardData> MissingEffects = new List<CardData>();

    public void Awake()
    {
        Singleton = this;
        // Force accessor to run
        cards = Cards;
    }
    
    public IEnumerable<Card> CardsBySlot (Slot slot, DeckList deckInfo) {
        foreach (Card c in Cards) {
            if (c.Slot == slot && deckInfo.Cards.Contains(c.Name)) {
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
            data.Notes = reader.GetField<string>("Notes");
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
            c.Text = data.Text;

            c.Effect = CardEffectTable.Map(data.Name, data.Text);
            if (c.Effect != null) {
                yield return c;
            }
            else
            {
                data.Name = data.Name.Replace(" ", "");
                MissingEffects.Add(data);
            }
        }
    }
}