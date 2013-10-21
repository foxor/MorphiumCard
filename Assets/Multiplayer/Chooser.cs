using UnityEngine;
using System.Collections;

public enum ModeSelection {
	Server,
	Client
}

public class ModeSelectionListener : EventListener<ModeSelection, EventData> {
	public static ModeSelectionListener Singleton = new ModeSelectionListener();
}

public class Chooser : MonoBehaviour {
	public void OnGUI() {
		if (GUI.Button(new Rect(0f, 0f, Screen.width / 2f, Screen.height), "Client")) {
			ModeSelectionListener.Singleton.Broadcast(ModeSelection.Client, null);
			Destroy(this);
		}
		if (GUI.Button(new Rect(Screen.width / 2f, 0f, Screen.width / 2f, Screen.height), "Server")) {
			ModeSelectionListener.Singleton.Broadcast(ModeSelection.Server, null);
			Destroy(this);
		}
	}
}