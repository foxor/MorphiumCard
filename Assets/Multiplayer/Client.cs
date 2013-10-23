using UnityEngine;
using System;
using System.Collections;

public class Client : MonoBehaviour {
	protected static GameObject prefab = (GameObject)Resources.Load("morphid");
	
	public static Client Singleton;
	
	public void Awake() {
		Singleton = this;
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Client, Connect);
	}
	
	protected void Connect(object ip) {
		Network.Connect((string)ip, Server.PORT);
		StartCoroutine(WaitForConnection());
	}
	
	protected IEnumerator WaitForConnection() {
		while (Network.peerType != NetworkPeerType.Client) {
			yield return 0;
		}
		
		Morphid.LocalPlayer = ((GameObject)Network.Instantiate(prefab, Vector3.zero, Quaternion.identity, 0)).GetComponent<Morphid>();
		networkView.RPC("SubmitPlayer", RPCMode.Server, Morphid.LocalPlayer.networkView.viewID);
	}
}