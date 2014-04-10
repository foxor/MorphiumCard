using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class CardPreview : CardUI {
    [NonSerialized]
    public Card Card;
    
    [NonSerialized]
    public Morphid Owner;
    
    protected Vector3 oldPos;
    protected Vector3 delta;
    protected bool selected;

    public override Card GetCard()
    {
        return Card;
    }
    
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
}