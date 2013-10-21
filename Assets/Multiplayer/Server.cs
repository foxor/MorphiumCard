using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {
	protected const int CONNECTIONS = 20;
	
	public const int PORT = 4141;
	
	public void Awake() {
		ModeSelectionListener.Singleton.AddCallback(ModeSelection.Server, Serve);
	}
	
	protected void Serve(EventData data) {
		Network.InitializeServer(CONNECTIONS, PORT, true);
	}
}