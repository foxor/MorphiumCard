using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpriteButton : SpriteRegion {
    public Action Action;
    
    public SpriteButton (GameObject x) : base(x) {
    }

    public override void Update () {
        if (ClickRaycast.MouseOverThis(Sprite) && Enabled && Input.GetMouseButtonDown(0)) {
            Action();
        }
        base.Update();
    }
}