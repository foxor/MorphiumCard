﻿using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class Client : MonoBehaviour {
	public static Client Singleton;
	
	public static string GUID;
	
	public void Awake() {
		Singleton = this;
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Client, Connect);
	}
	
	protected void Connect(object ip) {
		if (ip != null) {
			Network.Connect((string)ip, Server.PORT);
			StartCoroutine(WaitForConnection());
		}
		else {
			StartCoroutine(WaitForMasterServer());
		}
	}
	
	protected IEnumerator WaitForMasterServer() {
		MasterServer.RequestHostList(Server.MASTER_SERVER_NAME);
		HostData[] hosts;
		while ((hosts = MasterServer.PollHostList()).Length == 0) {
			yield return 0;
		}
		Network.Connect(hosts.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).First());
		StartCoroutine(WaitForConnection());
	}
	
	protected IEnumerator WaitForConnection() {
		while (Network.peerType != NetworkPeerType.Client) {
			yield return 0;
		}
		
		GUID = Guid.NewGuid().ToString();
		networkView.RPC("SubmitPlayer", RPCMode.Server, GUID);
	}
	
	[RPC]
	protected void AssignLocalPlayer(string id, NetworkViewID viewId) {
		Morphid morphid = NetworkView.Find(viewId).GetComponent<Morphid>();
		morphid.GUID = id;
		if (id == GUID) {
			Morphid.LocalPlayer = morphid;
		}
	}
	
	public static void PlayCard(Card c) {
		Singleton.networkView.RPC("ServerPlayCard", RPCMode.Server, Morphid.LocalPlayer.GUID, c.GUID);
	}
	
	public static void BoostEngine() {
		Singleton.networkView.RPC("ServerBoostEngine", RPCMode.Server, Morphid.LocalPlayer.GUID);
	}
	
	public static void DrawCard(int deck) {
		Singleton.networkView.RPC("ServerDrawCard", RPCMode.Server, Morphid.LocalPlayer.GUID, deck);
	}
}