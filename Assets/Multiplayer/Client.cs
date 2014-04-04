using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class Client : MonoBehaviour {
    public static Client Singleton;
    public static string GUID;
    
    public void Awake () {
        Singleton = this;
        Chooser.ChooseMode += OnChooseMode;
    }

    protected void OnChooseMode(ModeSelection modeSelection, string ip) {
        if (modeSelection == ModeSelection.Client) {
            Connect(ip);
        }
    }
    
    protected void Connect (object ip) {
        if (ip != null) {
            Network.Connect((string)ip, Server.PORT);
            StartCoroutine(WaitForConnection());
        } else {
            StartCoroutine(WaitForMasterServer());
        }
    }
    
    protected IEnumerator WaitForMasterServer () {
        MasterServer.RequestHostList(Server.MASTER_SERVER_NAME);
        HostData[] hosts;
        while ((hosts = MasterServer.PollHostList()).Length == 0) {
            yield return 0;
        }
        Network.Connect(hosts.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).First());
        StartCoroutine(WaitForConnection());
    }
    
    protected IEnumerator WaitForConnection () {
        while (Network.peerType != NetworkPeerType.Client) {
            yield return 0;
        }
        
        GUID = Guid.NewGuid().ToString();
        networkView.RPC("SubmitPlayer", RPCMode.Server, GUID);
    }
    
    public static void PlayCard (Card c, string targetGuid) {
        targetGuid = targetGuid == null ? "" : targetGuid;
        Singleton.networkView.RPC("ServerPlayCard", RPCMode.Server, Morphid.LocalPlayer.GUID, c.GUID, targetGuid);
    }
    
    public static void BoostEngine () {
        Singleton.networkView.RPC("ServerBoostEngine", RPCMode.Server, Morphid.LocalPlayer.GUID);
    }
}