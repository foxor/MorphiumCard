using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Server : MonoBehaviour {
    protected static GameObject gameStatePrefab = (GameObject)Resources.Load("GameState");
    protected static Server singleton;
    protected const int CONNECTIONS = 20;
    public const string MASTER_SERVER_NAME = "MorphiumCard";
    public const int PORT = 4141;
    protected List<string> Players;
    protected GameState GameState;
    protected string GameName;
    
    public void Awake () {
        singleton = this;
        ModeSelectionListener.Singleton.AddCallback(ModeSelection.Server, Serve);
        Players = new List<string> ();
    }
    
    protected void Serve (object data) {
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
    }
    
    private void EndTurn (Morphid enemy) {
        if (GameState.ActiveMorphid.Research != null) {
            GameState.ActiveMorphid.Research();
        }
        foreach (Lane l in GameState.Lanes) {
            Minion attacker = l.EnemyMinion(GameState.ActiveMorphid.GUID);
            Minion defender = l.FriendlyMinion(GameState.ActiveMorphid.GUID);
            if (attacker != null) {
                if (defender != null) {
                    networkView.RPC("QueueMinionAnimation", RPCMode.Others,
                        new MinionAnimation () {
                            AnimationType = AnimationType.AttackMinion,
                            GUID = attacker.GUID
                        }.SerializeProtoString()
                    );
                    networkView.RPC("QueueMinionHealthLie", RPCMode.Others,
                        new MinionHealthLie () {
                            GUID = defender.GUID,
                            Health = defender.Defense
                        }.SerializeProtoString()
                    );
                    networkView.RPC("QueueMinionAliveLie", RPCMode.Others,
                        new MinionAliveLie () {
                            GUID = defender.GUID,
                            Alive = true
                        }.SerializeProtoString()
                    );
                    GameState.DamageGuid(defender.GUID, attacker.Attack);
                } else {
                    if (!attacker.Defensive) {
                        GameState.DamageGuid(GameState.ActiveMorphid.GUID, attacker.Attack);
                    }
                }
            }
        }
        GameState.SwapTurn();
		foreach (Morphid m in GameState.Morphids) {
			m.RetemplateCards();
		}
        enemy.Morphium = Mathf.Min(Morphid.MAX_MORPHIUM, enemy.Morphium + enemy.Engine);
    }
    
    [RPC]
    public void ServerPlayCard (string morphidGuid, string cardGuid, string targetGuid) {
        GameState.GetMorphid(morphidGuid).PlayCard(cardGuid, targetGuid);
        EndTurn(GameState.GetEnemy(morphidGuid));
    }
    
    [RPC]
    public void ServerBoostEngine (string morphidGuid) {
        GameState.GetMorphid(morphidGuid).Engine += 1;
        GameState.ActiveMorphid.EngineSequence();
        EndTurn(GameState.GetEnemy(morphidGuid));
    }
}