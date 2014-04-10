using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class SpriteRegion {
    protected static Color EnabledColor = Color.white;
    protected static Color DisabledColor = new Color (1f, 1f, 1f, 0.1f);

    public GameObject Sprite;
    public TextMeshController TextArea;

    [NonSerialized]
    public bool Enabled = true;

    [NonSerialized]
    public bool Visible = true;
    
    protected string text;
    public string Text {
        get {
            return text;
        }
        set {
            text = value;
            ManageText();
        }
    }

    public bool ContainsMouse () {
        return ClickRaycast.MouseOverThis(Sprite);
    }

    protected virtual Color GetColor() {
        return Enabled ? EnabledColor : DisabledColor;
    }

    public virtual void Update () {
        Sprite.renderer.material.color = GetColor();
        Sprite.renderer.enabled = Visible;
        ManageText();
    }

    protected virtual void ManageText() {
        if (TextArea != null) {
            TextArea.Text = Text;
            TextArea.renderer.material.color = Sprite.renderer.material.color;
            TextArea.renderer.enabled = Sprite.renderer.enabled;
        }
    }
}