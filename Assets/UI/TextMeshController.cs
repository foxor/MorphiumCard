using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
[ExecuteInEditMode]
public class TextMeshController : MonoBehaviour {
    protected static char[] SEPERATORS = new char[]{' ', '\n', '\t'};
    
    public float MaxWidth = 100;
    public string Text = "";

    protected TextMesh textMesh;
    protected float lastMaxWidth;

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
    
    public void Update () {
        if (textMesh.text != Text || MaxWidth != lastMaxWidth) {
            float lineWidth = 0.0f;
            string[] words = Text.Split(SEPERATORS);
            float spaceWidth = GetSpaceWidth();

            Text = "";

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
                        if (lineWidth > 0) {
                            Text += '\n' + word;
                        }
                        else {
                            Text += word;
                        }
                        lineWidth = width;
                    }
                }
            }
            textMesh.text = Text;
            lastMaxWidth = MaxWidth;
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
    
    public void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position - (transform.right * MaxWidth / 2f), transform.right * MaxWidth);
    }
}