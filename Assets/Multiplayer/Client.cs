using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {
	public void Awake() {
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Client, Connect);
	}
	
	protected void Connect(EventData data) {
		Network.Connect("167.0.0.1", Server.PORT);
	}
}