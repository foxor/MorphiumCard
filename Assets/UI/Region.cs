using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Region {
	public enum Side {
		Left,
		Right,
		Top,
		Bottom
	}
	
	public enum Direction {
		Horizontal,
		Vertical
	}
	
	public int Left;
	public int Top;
	public int Width;
	public int Height;
	public bool Enabled;
	
	public bool invalid = true;
	protected Rect screenRect;
	public Rect ScreenRect {
		get {
			if (invalid) {
				screenRect = new Rect(
					(float)Left,
					(float)Top,
					(float)Width,
					(float)Height
				);
				invalid = false;
			}
			return screenRect;
		}
	}
	
	public List<Region> children;
	
	public Region() {
		Left = 0;
		Top = 0;
		Width = Screen.width;
		Height = Screen.height;
		children = new List<Region>();
		Enabled = true;
	}
	
	protected virtual void DrawInner() {
	}
	
	public void Draw() {
		bool orig = GUI.enabled;
		GUI.enabled &= Enabled;
		foreach (Region child in children) {
			child.Draw();
		}
		DrawInner();
		GUI.enabled = orig;
	}
	
	public Region Bisect(Side side, int size) {
		invalid = true;
		Region r = new Region() {
			Left = (side == Side.Right ? Left + Width - size : Left),
			Top = (side == Side.Bottom ? Top + Height - size : Top),
			Width = ((side == Side.Left || side == Side.Right) ? size : Width),
			Height = ((side == Side.Top || side == Side.Bottom) ? size : Height)
		};
		Left += (side == Side.Left ? size : 0);
		Top += (side == Side.Top ? size : 0);
		Width -= ((side == Side.Left || side == Side.Right) ? size : 0);
		Height -= ((side == Side.Top || side == Side.Bottom) ? size : 0);
		
		children.Add(r);
		return r;
	}
	
	public Region[] Split(Direction direction, int segments) {
		int mark = 0;
		Region[] regions = new Region[segments];
		for (int i = 0; i < segments; i++) {
			regions[i] = new Region() {
				Left = Left + (direction == Direction.Horizontal ? mark : 0),
				Top = Top + (direction == Direction.Vertical ? mark : 0),
				Width = (direction == Direction.Horizontal ? (Width * (i + 1)) / segments - mark : Width),
				Height = (direction == Direction.Vertical ? (Height * (i + 1)) / segments - mark : Height)
			};
			mark += (direction == Direction.Horizontal ? regions[i].Width : regions[i].Height);
			children.Add(regions[i]);
		}
		
		invalid = true;
		Width = 0;
		Height = 0;
		return regions;
	}
	
	public Region ContainsMouse() {
		Vector2 MousePos = Input.mousePosition;
		MousePos.y = Screen.height - MousePos.y;
		if (Left < MousePos.x && Left + Width > MousePos.x &&
			Top < MousePos.y && Top + Height > MousePos.y)
		{
			return this;
		}
		foreach (Region child in children) {
			Region found = child.ContainsMouse();
			if (found != null) {
				return found;
			}
		}
		return null;
	}
}