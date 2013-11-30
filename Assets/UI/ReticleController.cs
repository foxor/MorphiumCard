using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {
    protected static ReticleController singleton;
    protected static Vector3 delta = new Vector3 (0f, 0f, 8f);

    public void Awake () {
        singleton = this;
    }

    public void Update () {
        transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin + delta;
    }

    public static bool Shown {
        set {
            singleton.renderer.enabled = value;
        }
    }
}