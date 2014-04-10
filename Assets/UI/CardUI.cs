using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public abstract class CardUI : SpriteButton {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;

    protected static char[] Seperators = new char[]{' '};
    
    public TextMeshController CardCost;
    public TextMeshController CardName;

    public abstract Card GetCard();

    protected override Color GetColor()
    {
        if (GetCard() == null) {
            return Color.white;
        }
        Manufacturer manufacturer = (Manufacturer)Enum.Parse(typeof(Manufacturer), GetCard().Manufacturer.Split(Seperators)[0]);
        Color baseColor = Color.white;
        switch (manufacturer) {
        case Manufacturer.Central:
            baseColor = new Color(0.5f, 0.9f, 0,4f);
            break;
        case Manufacturer.Goliath:
            baseColor = new Color(0.6f, 0.3f, 0.2f);
            break;
        case Manufacturer.Cerberus:
            baseColor = new Color(0.9f, 0.7f, 0.4f);
            break;
        case Manufacturer.Pyrotech:
            baseColor = new Color(0.9f, 0.1f, 0f);
            break;
        case Manufacturer.Starpower:
            baseColor = new Color(0.6f, 0.2f, 0.7f);
            break;
        case Manufacturer.Vanguard:
            baseColor = new Color(0.4f, 0.4f, 0.8f);
            break;
        }
        if (!Enabled) {
            baseColor.a = 0.1f;
        }
        return baseColor;
    }
    
    protected override void ManageText () {
        if (GetCard() != null) {
            CardCost.Text = GetCard().Cost.ToString();
            CardName.Text = GetCard().Name;
            TextArea.Text = GetCard().Text;
            Sprite.renderer.enabled = true;
        } else {
            CardCost.Text = "";
            CardName.Text = "";
            TextArea.Text = "";
            Sprite.renderer.enabled = false;
        }
        CardCost.renderer.material.color = Sprite.renderer.material.color;
        CardName.renderer.material.color = Sprite.renderer.material.color;
        TextArea.renderer.material.color = Sprite.renderer.material.color;
    }
}