using UnityEngine;
using System.Collections;

public class SelectionRegion : Button {
	public Morphid Morphid;
	public Lane Lane;
	public Minion Minion;
		
	public SelectionRegion(Region source) : base(source) { }
}