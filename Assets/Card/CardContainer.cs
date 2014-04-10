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
    public Card[]
        Cards;
    [SerializeField]
    [ProtoMember(2)]
    public Deck[]
        Decks;
    
    static CardContainer () {
        RuntimeTypeModel.Default.Add(typeof(CardContainer), true)[1].SupportNull = true;
    }
    
    public Card FromGuid (string guid) {
        return Cards.Where(x => x != null && x.GUID == guid).Single();
    }

    public void Uncharge () {
        foreach (Card c in Cards) {
            if (c != null) {
                c.Charged = false;
            }
        }
    }
    
    public void ComboManufacturer (string Manufacturer) {
        foreach (Card c in Cards) {
            if (c != null && c.Manufacturer == Manufacturer) {
                c.Charged = true;
            }
        }
    }
    
    public void ComboSlot (Slot slot, bool charge = true) {
        foreach (Card c in Cards) {
            if (c != null && c.Slot == slot) {
                c.Charged = charge;
            }
        }
    }
    
    public void ReplaceCard (Card c) {
        for (int i = 0; i < Cards.Length; i++) {
            if (Cards[i] == c) {
                Cards[i] = Decks[i].Draw();
                if (Cards[i] != null) {
                    Cards[i].Template();
                }
            }
        }
    }

    public Card GetCard(Slot slot) {
        foreach (Card c in Cards) {
            if (c != null && c.Slot == slot) {
                return c;
            }
        }
        return null;
    }
    
    public void Setup (DeckList deckInfo) {
        Slot[] Slots = Enum.GetValues(typeof(Slot)).Cast<Slot>().OrderBy(x => x.Order()).ToArray();
        Cards = new Card[Slots.Count()];
        Decks = new Deck[Slots.Count()];
        for (int i = 0; i < Slots.Count(); i++) {
            Decks[i] = new Deck (){
                Slot = Slots[i]
            };
            Decks[i].Shuffle(deckInfo);
            Cards[i] = Decks[i].Draw();
            if (Cards[i] != null) {
                Cards[i].Charged = true;
            }
        }
    }
}