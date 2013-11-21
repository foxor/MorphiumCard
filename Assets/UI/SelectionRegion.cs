using UnityEngine;
using System.Collections;

public class SelectionRegion : SpriteRegion {
	public Morphid Morphid;
	public Lane Lane;
	public Minion Minion;
    
    public SelectionRegion (GameObject x) : base(x) {}
}