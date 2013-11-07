using UnityEngine;
using System.Collections;

public class Label : Region {
	public string Text;
	
	public Label(Region source) {
		source.children.Add(this);
		
		Left = source.Left;
		Top = source.Top;
		Width = source.Width;
		Height = source.Height;
		
		source.Width = 0;
		source.Height = 0;
		source.invalid = true;
	}
	
	protected override void DrawInner() {
		GUI.Label(ScreenRect, Text);
	}
}