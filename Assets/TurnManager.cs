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
	protected void SetActivePlayer(NetworkViewID view) {
		if (view.isMine) {
			isLocalActive = true;
		}
	}
	
	public void FinishTurn() {
		isLocalActive ^= true;
	}
}