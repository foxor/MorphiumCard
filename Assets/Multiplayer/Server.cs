using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Server : MonoBehaviour {
	protected static GameObject morphidPrefab = (GameObject)Resources.Load("morphid");
	protected static Server singleton;
	
	protected const int CONNECTIONS = 20;
	protected const int REQUIRED_PLAYERS = 2;
	
	public const string MASTER_SERVER_NAME = "MorphiumCard";
	public const int PORT = 4141;
	
	protected Dictionary<string, Morphid> Players;
	protected string GameName;
	
	public void Awake() {
		singleton = this;
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Server, Serve);
		Players = new Dictionary<string, Morphid>();
	}
	
	protected void Serve(object data) {
		Network.InitializeServer(CONNECTIONS, PORT, true);
		MasterServer.RegisterHost(MASTER_SERVER_NAME, GameName = Guid.NewGuid().ToString());
	}
	
	public void OnGUI() {
		if (Network.peerType == NetworkPeerType.Server) {
			GUILayout.Label("Serving at ip: " + Network.player.externalIP);
			GUILayout.Label(Players.Count + " players connected");
			GUILayout.Label("Game Name: " + GameName);
		}
	}
	
	[RPC]
	protected void SubmitPlayer(string guid) {
		GameObject newMorphid = (GameObject)Network.Instantiate(morphidPrefab, Vector3.zero, Quaternion.identity, 0);
		Players[guid] = newMorphid.GetComponent<Morphid>();
		Players[guid].GUID = guid;
		
		networkView.RPC("AssignLocalPlayer", RPCMode.Others, guid, 
			newMorphid.networkView.viewID
		);
		
		if (Players.Count >= REQUIRED_PLAYERS) {
			networkView.RPC("SetActivePlayer", RPCMode.All,
				Players.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).First().Key
			);
		}
	}
	
	public static Morphid GetMorphid(string guid) {
		return singleton.Players[guid];
	}
	
	public static Morphid GetEnemy(string guid) {
		return singleton.Players.Where(x => x.Key != guid).Single().Value;
	}
	
	[RPC]
	public void ServerPlayCard(string morphidGuid, string cardGuid) {
		GetMorphid(morphidGuid).CardContainer.FromGuid(cardGuid).Process(morphidGuid);
		networkView.RPC("FinishTurn", RPCMode.Others);
	}
}