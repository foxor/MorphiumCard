using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Morphid : MonoBehaviour {
	
	public const int MAX_HEALTH = 50;
	public const int MAX_MORPHIUM = 10;
	public const int START_MORPHIUM = 0;
	public const int START_ENGINE = 1;
	
	public static Morphid LocalPlayer;
	
	protected static Morphid remotePlayer;
	public static Morphid RemotePlayer {
		get {
			if (remotePlayer == null) {
				remotePlayer = FindObjectsOfType(typeof(Morphid)).Cast<Morphid>().Where(
					x => x.GUID != LocalPlayer.GUID
				).Single();
			}
			return remotePlayer;
		}
	}
	
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
	[SerializeField]
	public int Reflect = 0;
	
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
		stream.Serialize(ref Reflect);
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