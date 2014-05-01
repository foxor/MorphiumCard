using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class CardButton : CardUI {
    [NonSerialized]
    public bool SuspendDrag;

    [NonSerialized]
    public Morphid Owner;

    public int CardIndex;

    protected Vector3 oldPos;
    protected Vector3 delta;
    protected bool selected;

    public override Card GetCard() {
        return Morphid.Cards[CardIndex];
    }

    public void OnPickup () {
        oldPos = Sprite.transform.position;
        delta = oldPos - Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        selected = true;
    }

    public void OnDrop () {
        Sprite.transform.position = oldPos;
        Sprite.SetActive(false);
        selected = false;
    }

    public bool isEnabled () {
        return GetCard() != null &&
            GetCard().Cost <= Morphid.LocalPlayer.Morphium &&
            GetCard().Charged &&
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
}