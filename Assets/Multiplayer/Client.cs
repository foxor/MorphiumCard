using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class Client : MonoBehaviour {
    public static Client Singleton;
    public static string GUID;

    protected string ip;
    protected DeckList deckList;
    
    public void Awake () {
        Singleton = this;
        Chooser.ChooseMode += OnChooseMode;
        DeckBuilder.onChooseDeck += OnChooseDeck;
    }

    protected void OnChooseMode(ModeSelection modeSelection, string ip) {
        if (modeSelection == ModeSelection.Client) {
            this.ip = ip;
            DeckBuilder.Enable();
        }
    }

    protected void OnChooseDeck(DeckList deckList) {
        this.deckList = deckList;
        Connect();
    }
    
    protected void Connect () {
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

        GameBoard.Show();

        GUID = Guid.NewGuid().ToString();
        networkView.RPC("SubmitPlayer", RPCMode.Server, GUID, deckList.SerializeProtoString());
    }
    
    public static void PlayCard (string cardGuid, string targetGuid) {
        targetGuid = targetGuid == null ? "" : targetGuid;
        Singleton.networkView.RPC("ServerPlayCard", RPCMode.Server, Morphid.LocalPlayer.GUID, cardGuid, targetGuid);
    }
    
    public static void BoostEngine () {
        Singleton.networkView.RPC("ServerBoostEngine", RPCMode.Server, Morphid.LocalPlayer.GUID);
    }
}