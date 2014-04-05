using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class CardButton : SpriteButton {
    protected const int ART_WIDTH = 500;
    protected const int ART_HEIGHT = 770;

    [NonSerialized]
    public bool SuspendDrag;

    public int CardIndex;
    public TextMeshController CardCost;
    public TextMeshController CardName;

    protected Vector3 oldPos;
    protected Vector3 delta;
    protected bool selected;

    public void OnPickup () {
        oldPos = Sprite.transform.position;
        delta = oldPos - Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        selected = true;
    }

    public void OnDrop () {
        Sprite.transform.position = oldPos;
        selected = false;
    }

    public bool isEnabled () {
        return Morphid.Cards != null && 
            Morphid.Cards[CardIndex] != null &&
            Morphid.Cards[CardIndex].Cost <= Morphid.LocalPlayer.Morphium &&
            Morphid.Cards[CardIndex].Charged &&
            GameState.IsLocalActive;
    }
    
    public override void Update () {
        Enabled = isEnabled();
        if (selected && !SuspendDrag) {
            Sprite.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin + delta;
        }
        if (SuspendDrag) {
            Sprite.transform.position = oldPos;
        }
        base.Update();
    }

    protected override void ManageText () {
        if (Morphid.Cards != null && Morphid.Cards[CardIndex] != null) {
            CardCost.Text = Morphid.Cards[CardIndex].Cost.ToString();
            CardName.Text = Morphid.Cards[CardIndex].Name;
            TextArea.Text = Morphid.Cards[CardIndex].Text;
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