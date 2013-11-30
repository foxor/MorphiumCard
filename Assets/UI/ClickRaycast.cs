using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClickRaycast : MonoBehaviour {
    private static ClickRaycast singleton;
    private RaycastHit2D[] lastHit;
    private bool hitSomething;
    
    public static RaycastHit2D[] GetLastHit () {
        return singleton.lastHit;
    }
    
    public void Start () {
        singleton = this;
    }

    public static bool MouseOverThis (GameObject x) {
        return singleton.hitSomething && GetLastHit().Any(r => r.transform.gameObject == x);
    }
    
    public void Update () {
        if (Camera.main != null) {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            lastHit = Physics2D.RaycastAll(r.origin, r.direction);
            hitSomething = lastHit.Length > 0;
        }
    }
}
