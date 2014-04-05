using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class DeckBuilder : MonoBehaviour {
    public static Action<DeckList> onChooseDeck = (DeckList deckList) => {};
    
    protected static DeckBuilder Singleton;

    protected Dictionary<Slot, List<Card>> CardOptions;
    protected Dictionary<Card, bool> CardChecked;

    public void Awake() {
        Singleton = this;
    }

    public void Start() {
        CardOptions = new Dictionary<Slot, List<Card>>();
        CardChecked = new Dictionary<Card, bool>();

        foreach (Slot slot in Enum.GetValues(typeof(Slot))) {
            CardOptions[slot] = new List<Card>();
        }

        foreach (Card card in Importer.Singleton.Cards) {
            CardOptions[card.Slot].Add(card);
            CardChecked[card] = false;
        }
    }

    public static void Enable() {
        Singleton.enabled = true;
    }

    public void OnGUI() {
        GUILayout.BeginHorizontal();
        foreach (Slot slot in Enum.GetValues(typeof(Slot))) {
            GUILayout.BeginVertical();
            GUILayout.Label(slot.ToString() + ":");

            foreach (Card option in CardOptions[slot]) {
                CardChecked[option] = GUILayout.Toggle(CardChecked[option], option.Name);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Submit")) {
            DeckList deckList = new DeckList();
            deckList.Cards = CardChecked.Where(x => x.Value).Select((pair, _) => pair.Key.Name).ToArray();
            onChooseDeck(deckList);
            this.enabled = false;
        }
    }
}