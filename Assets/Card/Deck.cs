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
    protected static Importer Importer = new Importer ();
    [SerializeField]
    [ProtoMember(1)]
    public Card[]
        Cards;
    [SerializeField]
    [ProtoMember(2)]
    public int
        Index;
    [SerializeField]
    [ProtoMember(3)]
    public Slot
        Slot;
    
    public void Shuffle () {
        Cards = Importer.CardsBySlot(Slot).OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToArray();
    }
    
    public Card Draw () {
        if (Index >= Cards.Length) {
            Cards = Cards.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToArray();
            Index = 0;
        }
        if (Cards != null && Cards.Length > Index) {
            return Cards[Index++];
        }
        return null;
    }
}