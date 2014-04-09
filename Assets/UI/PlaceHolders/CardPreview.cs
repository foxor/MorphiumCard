using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class CardPreview : SpriteButton {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;
    
    [NonSerialized]
    public Card Card;
    
    [NonSerialized]
    public Morphid Owner;

    public TextMeshController CardCost;
    public TextMeshController CardName;
    
    protected Vector3 oldPos;
    protected Vector3 delta;
    protected bool selected;
    
    public void SnapToMouse() {
        Sprite.transform.position += Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        oldPos = Sprite.transform.position;
        delta = oldPos - Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        selected = true;
    }
    
    public override void Update () {
        Enabled = true;
        if (selected) {
            Sprite.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin + delta;
        }
        base.Update();
    }
    
    protected override void ManageText () {
        if (Card != null) {
            CardCost.Text = Card.Cost.ToString();
            CardName.Text = Card.Name;
            TextArea.Text = Card.Text;
            Sprite.renderer.enabled = true;
        } else {
            CardCost.Text = "";
            CardName.Text = "";
            TextArea.Text = "";
            Sprite.renderer.enabled = false;
        }
    }
}