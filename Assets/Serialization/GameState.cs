using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GameState : MonoBehaviour {
	protected const int NUM_PLAYERS = 2;
	protected const int NUM_LANES = 3;
	
	protected static GameState singelton;
	
	public int PlayerCount;
	public Morphid[] Morphids;
	public Lane[] Lanes;
	
	protected int ActivePlayer;
	
	public void Awake() {
		singelton = this;
		if (Network.peerType == NetworkPeerType.Client) {
			StartCoroutine(SetupCoroutine());
		}
		Morphids = new Morphid[NUM_PLAYERS].Select(x => new Morphid()).ToArray();
		Lanes = new Lane[NUM_LANES].Select(x => new Lane()).ToArray();
	}
	
	public void AddMorphid(string guid) {
		Morphids[PlayerCount].GUID = guid;
		Morphids[PlayerCount].CardContainer.Setup();
		
		if (++PlayerCount >= NUM_PLAYERS) {
			ActivePlayer = UnityEngine.Random.Range(0, 1);
		}
	}
	
	protected IEnumerator SetupCoroutine() {
		while (PlayerCount < NUM_PLAYERS) {
			yield return 0;
		}
	}
	
	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.SerializeProto<Morphid[]>(ref Morphids);
		stream.SerializeProto<Lane[]>(ref Lanes);
		stream.SerializeProto<int>(ref ActivePlayer);
		stream.SerializeProto<int>(ref PlayerCount);
	}
	
	public static bool IsLocalActive {
		get {
			return singelton.PlayerCount >= NUM_PLAYERS &&
				singelton.Morphids[singelton.ActivePlayer].GUID == Client.GUID;
		}
	}
	
	public void SwapTurn() {
		ActivePlayer = (ActivePlayer + 1) % NUM_PLAYERS;
	}
	
	public static Morphid GetMorphid(string guid) {
		return singelton == null || guid == null ? null : singelton.Morphids.Where(x => x.GUID == guid).SingleOrDefault();
	}
	
	public static Morphid GetEnemy(string guid) {
		return singelton == null || guid == null ? null : singelton.Morphids.Where(x => x.GUID != guid).SingleOrDefault();
	}
}