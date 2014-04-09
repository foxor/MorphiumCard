using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
[ExecuteInEditMode]
public class TextMeshController : MonoBehaviour {
    protected static char[] SEPERATORS = new char[]{' ', '\n', '\t'};
    
    public float MaxWidth = 100;
    public float MaxHeight = 20;
    public int MaxFontSize = 12;

    public string text;
    public string Text {
        get {
            return text;
        }
        set {
            if (value != lastText) {
                lastText = value;
                text = value;
            }
        }
    }

    protected TextMesh textMesh;
    protected string lastText;
    protected float lastMaxWidth;
    protected float lastMaxHeight;
    protected int lastMaxFontSize;
    protected string[] words;

    public float TextWidth {
        get{ return textMesh.renderer.bounds.extents.x * 2.0f; }
    }
    
    public float TextHeight {
        get{ return textMesh.renderer.bounds.extents.y * 2.0f; }
    }

    public void Awake () {
        textMesh = GetComponent<TextMesh>();
        Text = textMesh.text;
    }

    protected bool Fit() {
        Text = "";
        float lineWidth = 0.0f;
        float spaceWidth = GetSpaceWidth();
        
        for (int i = 0; i < words.Length; i++) {
            string word = words[i].Trim();
            float width = GetWordWidth(word);
            if (i == 0) {
                Text = word;
                lineWidth = width;
            } else {
                if (lineWidth + width + spaceWidth <= MaxWidth) {
                    Text += ' ' + word;
                    lineWidth += spaceWidth + width;
                }
                else {
                    if (width > MaxWidth) {
                        return false;
                    }
                    Text += '\n' + word;
                    lineWidth = width;
                }
            }
        }
        textMesh.text = Text;
        return GetTotalHeight() <= MaxHeight;
    }
    
    public void Update () {
        if (textMesh.text != Text || MaxWidth != lastMaxWidth || MaxHeight != lastMaxHeight || MaxFontSize != lastMaxFontSize) {
            words = Text.Split(SEPERATORS);

            int fontSize;

            textMesh.fontSize = MaxFontSize;

            if (!Fit()) {
                int minFontSize = 0;
                int maxFontSize = MaxFontSize;
                while (minFontSize + 1 < maxFontSize) {
                    fontSize = (minFontSize + maxFontSize) / 2;
                    textMesh.fontSize = fontSize;

                    if (Fit()) {
                        minFontSize = fontSize;
                    }
                    else {
                        maxFontSize = fontSize;
                    }
                }

                fontSize = minFontSize;
                textMesh.fontSize = fontSize;
                Fit();
            }

            lastMaxFontSize = MaxFontSize;
            lastMaxWidth = MaxWidth;
            lastMaxHeight = MaxHeight;
        }
    }

    private float GetSpaceWidth () {
        textMesh.text = "x x";
        float totalWidth = textMesh.renderer.bounds.extents.x * 2.0f;
        totalWidth -= GetWordWidth("xx");
        return totalWidth;
    }
    
    private float GetWordWidth (string word) {
        textMesh.text = word;
        return textMesh.renderer.bounds.extents.x * 2.0f;
    }
    
    private float GetTotalHeight () {
        return textMesh.renderer.bounds.extents.y * 2.0f;
    }
    
    public void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position - (transform.right * MaxWidth / 2f), transform.right * MaxWidth);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - (transform.up * MaxHeight / 2f), transform.up * MaxHeight);
    }
}