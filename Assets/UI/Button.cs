using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Button : Region {
    public string Text;
    public Action Action;
    
    public Button (Region source) {
        source.children.Add(this);
        
        Left = source.Left;
        Top = source.Top;
        Width = source.Width;
        Height = source.Height;
        
        source.Width = 0;
        source.Height = 0;
        source.invalid = true;
    }
    
    protected override void DrawInner () {
        if (GUI.Button(ScreenRect, Text)) {
            Action();
        }
    }
}