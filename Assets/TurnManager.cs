using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {
	protected static bool isLocalActive = false;
	public static bool IsLocalActive {
		get {
			return isLocalActive;
		}
	}
	
	[RPC]
	protected void SetActivePlayer(string guid) {
		if (guid == Client.GUID) {
			isLocalActive = true;
		}
	}
	
	[RPC]
	public void FinishTurn() {
		isLocalActive ^= true;
	}
}