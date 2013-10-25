using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Server : MonoBehaviour {
	protected static GameObject morphidPrefab = (GameObject)Resources.Load("morphid");
	
	protected const int CONNECTIONS = 20;
	protected const int REQUIRED_PLAYERS = 2;
	
	public const string MASTER_SERVER_NAME = "MorphiumCard";
	public const int PORT = 4141;
	
	protected List<string> Players;
	protected string GameName;
	
	public void Awake() {
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Server, Serve);
		Players = new List<string>();
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
		Players.Add(guid);
		
		networkView.RPC("AssignLocalPlayer", RPCMode.Others, guid, 
			((GameObject)Network.Instantiate(morphidPrefab, Vector3.zero, Quaternion.identity, 0)).networkView.viewID
		);
		
		if (Players.Count >= REQUIRED_PLAYERS) {
			networkView.RPC("SetActivePlayer", RPCMode.All,
				Players.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).First()
			);
		}
	}
}