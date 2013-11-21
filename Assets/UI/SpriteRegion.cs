using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class SpriteRegion {
    protected static Color EnabledColor = Color.white;
    protected static Color DisabledColor = new Color(1f, 1f, 1f, 0.1f);

    protected GameObject Sprite;
    protected TextMesh TextAreas;

    public string Text;
    public bool Enabled;
    public bool Visible;

    public SpriteRegion(GameObject Sprite) {
        this.Sprite = Sprite;
        TextAreas = Sprite.GetComponentInChildren<TextMesh>();
        Enabled = true;
        Visible = true;
    }

    public bool ContainsMouse () {
        return ClickRaycast.MouseOverThis(Sprite);
    }

    public virtual void Update () {
        Sprite.renderer.material.color = Enabled ? EnabledColor : DisabledColor;
        Sprite.renderer.enabled = Visible;
        if (TextAreas != null) {
            TextAreas.text = Text;
        }
    }
}