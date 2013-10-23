using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Server : MonoBehaviour {
	protected const int CONNECTIONS = 20;
	protected const int REQUIRED_PLAYERS = 2;
	
	public const int PORT = 4141;
	
	protected List<NetworkViewID> Players;
	
	public void Awake() {
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Server, Serve);
		Players = new List<NetworkViewID>();
	}
	
	protected void Serve(object data) {
		Network.InitializeServer(CONNECTIONS, PORT, true);
	}
	
	public void OnGUI() {
		if (Network.peerType == NetworkPeerType.Server) {
			GUILayout.Label("Serving at ip: " + Network.player.externalIP);
			GUILayout.Label(Players.Count + " players connected");
		}
	}
	
	[RPC]
	protected void SubmitPlayer(NetworkViewID view) {
		Players.Add(view);
		
		if (Players.Count >= REQUIRED_PLAYERS) {
			networkView.RPC("SetActivePlayer", RPCMode.All,
				Players.OrderBy(x => Random.Range(0f, 1f)).First()
			);
		}
	}
}