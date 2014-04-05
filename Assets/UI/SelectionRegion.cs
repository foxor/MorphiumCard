using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class SelectionRegion : SpriteRegion {
    [NonSerialized]
    public Morphid Morphid;

    [NonSerialized]
    public Lane Lane;

    [NonSerialized]
    public Minion Minion;
}