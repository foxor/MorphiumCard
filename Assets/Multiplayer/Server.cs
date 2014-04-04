using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Server : MonoBehaviour {
    protected static GameObject gameStatePrefab;
    protected static Server singleton;
    protected const int CONNECTIONS = 20;
    public const string MASTER_SERVER_NAME = "MorphiumCard";
    public const int PORT = 4141;
    protected List<string> Players;
    protected GameState GameState;
    protected string GameName;
    
    public void Awake () {
        gameStatePrefab = (GameObject)Resources.Load("GameState");
        singleton = this;
        Players = new List<string> ();
        Chooser.ChooseMode += OnChooseMode;
    }
    
    protected void OnChooseMode(ModeSelection modeSelection, string ip) {
        if (modeSelection == ModeSelection.Server) {
            Serve();
        }
    }
    
    protected void Serve () {
        Network.InitializeServer(CONNECTIONS, PORT, true);
        MasterServer.RegisterHost(MASTER_SERVER_NAME, GameName = Guid.NewGuid().ToString());
        GameState = ((GameObject)Network.Instantiate(gameStatePrefab, Vector3.zero, Quaternion.identity, 0)).GetComponent<GameState>();
    }
    
    public void OnGUI () {
        if (Network.peerType == NetworkPeerType.Server) {
            GUILayout.Label("Serving at ip: " + Network.player.externalIP);
            GUILayout.Label(Players.Count + " players connected");
            GUILayout.Label("Game Name: " + GameName);
        }
    }
    
    [RPC]
    protected void SubmitPlayer (string guid) {
        Players.Add(guid);
        GameState.AddMorphid(guid);
        GameState.GetMorphid(guid).Retemplate();
    }
    
    private void EndTurn (Morphid enemy) {
        if (GameState.ActiveMorphid.Research != null) {
            GameState.ActiveMorphid.Research();
        }
        GameState.SwapTurn();
        foreach (Minion minion in GameState.GetMinions()) {
            minion.OnTurnBegin();
        }
        GameStateWatcher.OnPostAttack(GameState.ActiveMorphid.GUID);
        foreach (Morphid morphid in GameState.Singleton.Morphids) {
            morphid.OnTurnBegin();
        }
    }
    
    [RPC]
    public void ServerPlayCard (string morphidGuid, string cardGuid, string targetGuid) {
        GameState.GetMorphid(morphidGuid).PlayCard(cardGuid, targetGuid);
        EndTurn(GameState.GetEnemy(morphidGuid));
    }
    
    [RPC]
    public void ServerBoostEngine (string morphidGuid) {
        GameState.AddEngine(morphidGuid, 1);
		if (GameState.ActiveMorphid.EngineSequence != null) {
        	GameState.ActiveMorphid.EngineSequence();
		}
        EndTurn(GameState.GetEnemy(morphidGuid));
    }
}