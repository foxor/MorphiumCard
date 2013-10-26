using UnityEngine;
using System;
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
	public int Health = 10;
	[SerializeField]
	public int Morphium = 10;
	
	[HideInInspector]
	public string GUID;
	
	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.SerializeProto<CardContainer>(ref CardContainer);
		stream.Serialize(ref Health);
		stream.Serialize(ref Morphium);
	}
	
	public static Action PlayLocalCardFunction(int card) {
		return () => {
			Client.PlayCard(Morphid.Cards[card]);
		};
	}
}