using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Morphid {
	
	public const int MAX_HEALTH = 50;
	public const int MAX_MORPHIUM = 10;
	public const int START_MORPHIUM = 0;
	public const int START_ENGINE = 1;
	
	public static Morphid LocalPlayer {
		get {
			return GameState.GetMorphid(Client.GUID);
		}
	}
	public static Morphid RemotePlayer {
		get {
			return GameState.GetEnemy(Client.GUID);
		}
	}
	
	[SerializeField]
	[ProtoMember(1)]
	public CardContainer CardContainer = new CardContainer();
	
	public static Card[] Cards {
		get {
			return LocalPlayer.CardContainer.Cards;
		}
	}
	
	[SerializeField]
	[ProtoMember(2)]
	public int Health;
	
	[SerializeField]
	[ProtoMember(3)]
	public int Morphium;
	
	[SerializeField]
	[ProtoMember(4)]
	public int Engine;
	
	[SerializeField]
	[ProtoMember(5)]
	public int Reflect;
	
	[HideInInspector]
	[SerializeField]
	[ProtoMember(6)]
	public string GUID;
	
	public Morphid() {
		Health = MAX_HEALTH;
		Morphium = START_MORPHIUM;
		Engine = START_ENGINE;
		Reflect = 0;
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