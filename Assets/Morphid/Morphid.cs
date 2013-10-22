using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Morphid : MonoBehaviour {
	
	public static Morphid LocalPlayer;
	
	[SerializeField]
	public CardContainer CardContainer;
	
	public static Card[] Cards {
		get {
			return LocalPlayer.CardContainer.Cards;
		}
	}
	
	[SerializeField]
	public ItemContainer ItemContainer;
	
	public static Item[] Items {
		get {
			return LocalPlayer.ItemContainer.Items;
		}
	}
	
	public void Awake() {
		CardContainer = new CardContainer(){Cards = new Card[] {
				new Card(){Name = "Card 1"},
				new Card(){Name = "Card 1"},
				new Card(){Name = "Card 1"},
				new Card(){Name = "Card 1"}
			}
		};
		
		ItemContainer = new ItemContainer(){
		};
	}
	
	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.SerializeProto<CardContainer>(ref CardContainer);
		stream.SerializeProto<ItemContainer>(ref ItemContainer);
	}
}