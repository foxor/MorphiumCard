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
	
	protected void Serve(EventData data) {
		Network.InitializeServer(CONNECTIONS, PORT, true);
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