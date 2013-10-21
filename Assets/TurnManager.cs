using UnityEngine;
using System.Collections;

public class TurnManager {
	protected static bool isLocalActive = false;
	public static bool IsLocalActive {
		get {
			return isLocalActive;
		}
		set {
			isLocalActive = value;
		}
	}
}