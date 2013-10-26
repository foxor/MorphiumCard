using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Morphid : MonoBehaviour {
	
	public const int MAX_HEALTH = 10;
	public const int MAX_MORPHIUM = 10;
	
	public static Morphid LocalPlayer;
	
	[SerializeField]
	public CardContainer CardContainer;
	
	public static Card[] Cards {
		get {
			return LocalPlayer.CardContainer.Cards;
		}
	}
	
	[SerializeField]
	public int Health = MAX_HEALTH;
	[SerializeField]
	public int Morphium = MAX_MORPHIUM;
	
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