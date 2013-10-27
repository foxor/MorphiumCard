using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Morphid : MonoBehaviour {
	
	public const int MAX_HEALTH = 10;
	public const int MAX_MORPHIUM = 10;
	public const int START_MORPHIUM = 0;
	public const int START_ENGINE = 1;
	
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
	public int Morphium = START_MORPHIUM;
	[SerializeField]
	public int Engine = START_ENGINE;
	
	[HideInInspector]
	public string GUID;
	
	public void Awake() {
		CardContainer.Setup();
	}
	
	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.SerializeProto<CardContainer>(ref CardContainer);
		stream.Serialize(ref Health);
		stream.Serialize(ref Morphium);
		stream.Serialize(ref Engine);
	}
	
	public static Action PlayLocalCardFunction(int card) {
		return () => {
			Client.PlayCard(Morphid.Cards[card]);
		};
	}
	
	public void PlayCard(string cardGuid) {
		Card c = CardContainer.FromGuid(cardGuid);
		c.Process(GUID);
		CardContainer.RemoveBroken();
	}
}