using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GameState : MonoBehaviour {
	protected const int REQUIRED_PLAYERS = 2;
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
		else {
			Morphids = new Morphid[REQUIRED_PLAYERS];
			Lanes = new Lane[NUM_LANES].Select(x => new Lane()).ToArray();
		}
	}
	
	public void AddMorphid(string guid) {
		Morphid m = Morphids[PlayerCount++] = new Morphid() {
			GUID = guid
		};
		m.CardContainer.Setup();
		
		if (PlayerCount >= REQUIRED_PLAYERS) {
			ActivePlayer = UnityEngine.Random.Range(0, 1);
		}
	}
	
	protected IEnumerator SetupCoroutine() {
		while (PlayerCount <= REQUIRED_PLAYERS) {
			yield return 0;
		}
		
		foreach (Morphid morphid in Morphids) {
			if (morphid.GUID == Client.GUID) {
				Morphid.LocalPlayer = morphid;
			}
			else {
				Morphid.RemotePlayer = morphid;
			}
		}
	}
	
	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.SerializeProto<Morphid[]>(ref Morphids);
		stream.SerializeProto<Lane[]>(ref Lanes);
		stream.SerializeProto<int>(ref ActivePlayer);
	}
	
	public static bool IsLocalActive {
		get {
			return singelton.PlayerCount >= REQUIRED_PLAYERS &&
				singelton.Morphids[singelton.ActivePlayer].GUID == Client.GUID;
		}
	}
	
	public static Morphid GetMorphid(string guid) {
		return singelton.Morphids.Where(x => x.GUID == guid).Single();
	}
	
	public static Morphid GetEnemy(string guid) {
		return singelton.Morphids.Where(x => x.GUID != guid).Single();
	}
}