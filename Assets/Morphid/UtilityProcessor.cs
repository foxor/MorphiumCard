using UnityEngine;
using System.Collections;

public class UtilityProcessor : MonoBehaviour {
	
	protected static UtilityProcessor Singleton;
	
	protected Card processing;
	
	public void Awake() {
		Singleton = this;
	}
	
	public static void Process(Card c) {
		if (c.Type == CardType.Attack) {
			Singleton.processing = c;
		}
	}
}