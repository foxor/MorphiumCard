using UnityEngine;
using System.Collections;

public class TextFieldMarker : MonoBehaviour {
    public string Text {
        set {
            text.Text = value;
        }
    }
    
    protected TextMeshController text;
    
    public void Awake () {
        text = GetComponent<TextMeshController>();
    }
}