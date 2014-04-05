using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SpriteButton : SpriteRegion {
    public Action Action;

    public override void Update () {
        if (ClickRaycast.MouseOverThis(Sprite) && Enabled && Input.GetMouseButtonDown(0)) {
            Action();
        }
        base.Update();
    }
}