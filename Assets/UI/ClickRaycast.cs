using UnityEngine;
using System;
using System.Collections.Generic;

public class ClickRaycast : MonoBehaviour {
	private static ClickRaycast singleton;
	
	private RaycastHit2D lastHit;
	private bool hitSomething;
	
	public static RaycastHit2D GetLastHit() {
		return singleton.lastHit;
	}
	
	public void Start() {
		singleton = this;
	}

    public static bool ClickedThis(GameObject x)
    {
        return singleton.hitSomething && GetLastHit().transform.gameObject == x;
    }
	
	public void Update() {
		if (Camera.main != null) {
			if (Input.GetMouseButton(0)) {
				Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
				lastHit = Physics2D.Raycast(r.origin, r.direction);
				hitSomething = lastHit.collider != null;
			}
			else {
				hitSomething = false;
			}
		}
	}
}
