using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GlobalRequire: MonoBehaviour {
	public void Awake() {
		gameObject.AddComponent<UI>();
		gameObject.AddComponent<TurnManager>();
		gameObject.AddComponent<Server>();
		gameObject.AddComponent<Client>();
		gameObject.AddComponent<Chooser>();
	}
}