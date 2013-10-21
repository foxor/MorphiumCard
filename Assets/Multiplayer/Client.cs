using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {
	public void Awake() {
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Client, Connect);
	}
	
	protected void Connect(EventData data) {
		Network.Connect("127.0.0.1", Server.PORT);
		StartCoroutine(WaitForConnection());
	}
	
	protected IEnumerator WaitForConnection() {
		while (Network.peerType != NetworkPeerType.Client) {
			yield return 0;
		}
		Debug.Log("Connected!");
	}
}