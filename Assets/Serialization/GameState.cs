using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GameState : MonoBehaviour {
	protected const int NUM_PLAYERS = 2;
	protected const int NUM_LANES = 3;
	
	public static GameState Singelton;
	
	public int PlayerCount;
	public Morphid[] Morphids;
	public Lane[] Lanes;
	
	protected int ActivePlayer;
	
	public void Awake() {
		Singelton = this;
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
			return Singelton.PlayerCount >= NUM_PLAYERS &&
				Singelton.Morphids[Singelton.ActivePlayer].GUID == Client.GUID;
		}
	}
	
	public void SwapTurn() {
		ActivePlayer = (ActivePlayer + 1) % NUM_PLAYERS;
	}
	
	public static Morphid GetMorphid(string guid) {
		return Singelton == null || guid == null ? null : Singelton.Morphids.Where(x => x.GUID == guid).SingleOrDefault();
	}
	
	public static Morphid GetEnemy(string guid) {
		return Singelton == null || guid == null ? null : Singelton.Morphids.Where(x => x.GUID != guid).SingleOrDefault();
	}
	
	public static Lane GetLane(int lane) {
		return Singelton == null ? null : Singelton.Lanes[lane];
	}
}