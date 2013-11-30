using UnityEngine;
using System.Collections;

public enum ModeSelection {
    Server,
    Client
}

public class ModeSelectionListener : EventListener<ModeSelection> {
    public static ModeSelectionListener Singleton = new ModeSelectionListener ();
}

public class Chooser : MonoBehaviour {
    protected string IP = "127.0.0.1";
    
    public void OnGUI () {
        IP = GUI.TextArea(new Rect (0f, 0f, Screen.width / 2f, 20f), IP);
        if (GUI.Button(new Rect (0f, 20f, Screen.width / 2f, Screen.height / 2f - 20f), "Client By IP")) {
            ModeSelectionListener.Singleton.Broadcast(ModeSelection.Client, IP);
            Destroy(this);
        }
        if (GUI.Button(new Rect (0f, Screen.height / 2f, Screen.width / 2f, Screen.height / 2f), "Client from master server")) {
            ModeSelectionListener.Singleton.Broadcast(ModeSelection.Client, null);
            Destroy(this);
        }
        if (GUI.Button(new Rect (Screen.width / 2f, 0f, Screen.width / 2f, Screen.height), "Server")) {
            ModeSelectionListener.Singleton.Broadcast(ModeSelection.Server, null);
            Destroy(this);
        }
    }
}