using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Server : MonoBehaviour {
	protected static GameObject gameStatePrefab = (GameObject)Resources.Load("gameState");
	protected static Server singleton;
	
	protected const int CONNECTIONS = 20;
	
	public const string MASTER_SERVER_NAME = "MorphiumCard";
	public const int PORT = 4141;
	
	protected List<string> Players;
	protected GameState GameState;
	protected string GameName;
	
	public void Awake() {
		singleton = this;
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Server, Serve);
		Players = new List<string>();
	}
	
	protected void Serve(object data) {
		Network.InitializeServer(CONNECTIONS, PORT, true);
		MasterServer.RegisterHost(MASTER_SERVER_NAME, GameName = Guid.NewGuid().ToString());
		GameState = ((GameObject)Network.Instantiate(gameStatePrefab, Vector3.zero, Quaternion.identity, 0)).GetComponent<GameState>();
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
		GameState.AddMorphid(guid);
	}
	
	private void EndTurn(Morphid enemy) {
		networkView.RPC("FinishTurn", RPCMode.Others);
		enemy.Morphium = Mathf.Min(Morphid.MAX_MORPHIUM, enemy.Morphium + enemy.Engine);
	}
	
	[RPC]
	public void ServerPlayCard(string morphidGuid, string cardGuid) {
		GameState.GetMorphid(morphidGuid).PlayCard(cardGuid);
		EndTurn(GameState.GetEnemy(morphidGuid));
	}
	
	[RPC]
	public void ServerBoostEngine(string morphidGuid) {
		GameState.GetMorphid(morphidGuid).Engine += 1;
		EndTurn(GameState.GetEnemy(morphidGuid));
	}
	
	[RPC]
	public void ServerDrawCards(string morphidGuid) {
		GameState.GetMorphid(morphidGuid).CardContainer.Draw();
		EndTurn(GameState.GetEnemy(morphidGuid));
	}
}