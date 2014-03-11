using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class SpriteRegion {
    protected static Color EnabledColor = Color.white;
    protected static Color DisabledColor = new Color (1f, 1f, 1f, 0.1f);
    protected GameObject sprite;
    protected TextMesh TextArea;
    public string Text;
    public bool Enabled;
    public bool Visible;

    public GameObject Sprite {
        get {
            return sprite;
        }
    }

    public SpriteRegion (GameObject Sprite) {
        this.sprite = Sprite;
        TextArea = Sprite.GetComponentInChildren<TextMesh>();
        Enabled = true;
        Visible = true;
    }

    public bool ContainsMouse () {
        return ClickRaycast.MouseOverThis(Sprite);
    }

    public virtual void Update () {
        Sprite.renderer.material.color = Enabled ? EnabledColor : DisabledColor;
        Sprite.renderer.enabled = Visible;
        ManageText();
    }

    protected virtual void ManageText() {
        if (TextArea != null) {
            TextArea.text = Text;
            TextArea.renderer.material.color = Sprite.renderer.material.color;
            TextArea.renderer.enabled = Sprite.renderer.enabled;
        }
    }
}