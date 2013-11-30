using UnityEngine;
using System.Collections;

public class MorphidMarker : MonoBehaviour {
    public string Text {
        set {
            text.text = value;
        }
    }
    
    protected TextMesh text;
    
    public void Awake () {
        text = GetComponent<TextMesh>();
    }
}