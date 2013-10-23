﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class UI : MonoBehaviour {
	protected Button[] Cards;
	protected Button[] Stats;
	
	protected class Region {
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
		}
		
		public virtual void Draw() {
			foreach (Region child in children) {
				child.Draw();
			}
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
	}
	
	protected class Button : Region {
		public string Text;
		public Action Action;
		
		public Button(Region source) {
			source.children.Add(this);
			
			Left = source.Left;
			Top = source.Top;
			Width = source.Width;
			Height = source.Height;
			
			source.Width = 0;
			source.Height = 0;
			source.invalid = true;
		}
		
		public override void Draw () {
			base.Draw ();
			if(GUI.Button(ScreenRect, Text)) {
				Action();
			}
		}
	}
	
	protected Region root;
	
	public void Awake() {
		root = new Region();
		Region CriticalStatsLayer = root.Bisect(Region.Side.Bottom, 20);
		Region OtherStatLayer = root.Bisect(Region.Side.Bottom, 20);
		Region CardLayer = root.Bisect(Region.Side.Bottom, 20);
		Region DrawLayer = root.Bisect(Region.Side.Bottom, 15);
		
		Region[] CriticalStatRegions = CriticalStatsLayer.Split(Region.Direction.Horizontal, 2);
		Region[] OtherStatRegions = OtherStatLayer.Split(Region.Direction.Horizontal, 5);
		Region[] CardRegions = CardLayer.Split(Region.Direction.Horizontal, 4);
		Region[] DrawRegions = DrawLayer.Split(Region.Direction.Horizontal, 4);
		
		Stats = new Button[7];
		Cards = new Button[4];
		
		Stats[0] = new Button(CriticalStatRegions[0]) {
			Text = "Health",
			Action = () => {}
		};
		Stats[1] = new Button(CriticalStatRegions[1]) {
			Text = "Morphium",
			Action = () => {}
		};
		
		Stats[2] = new Button(OtherStatRegions[0]) {
			Text = "Torque",
			Action = () => {}
		};
		Stats[3] = new Button(OtherStatRegions[1]) {
			Text = "Sensors",
			Action = () => {}
		};
		Stats[4] = new Button(OtherStatRegions[2]) {
			Text = "Attack",
			Action = () => {}
		};
		Stats[5] = new Button(OtherStatRegions[3]) {
			Text = "Bandwidth",
			Action = () => {}
		};
		Stats[6] = new Button(OtherStatRegions[4]) {
			Text = "Speed",
			Action = () => {}
		};
		
		Cards[0] = new Button(CardRegions[0]) {
			Text = "Card 1",
			Action = () => {}
		};
		Cards[1] = new Button(CardRegions[1]) {
			Text = "Card 2",
			Action = () => {}
		};
		Cards[2] = new Button(CardRegions[2]) {
			Text = "Card 3",
			Action = () => {}
		};
		Cards[3] = new Button(CardRegions[3]) {
			Text = "Card 4",
			Action = () => {}
		};
		
		new Button(DrawRegions[0]) {
			Text = "Draw",
			Action = () => {}
		};
		new Button(DrawRegions[1]) {
			Text = "Draw",
			Action = () => {}
		};
		new Button(DrawRegions[2]) {
			Text = "Draw",
			Action = () => {}
		};
		new Button(DrawRegions[3]) {
			Text = "Draw",
			Action = () => {}
		};
	}
	
	public void OnGUI() {
		if (TurnManager.IsLocalActive) {
			for (int i = 0; i < Morphid.Cards.Length; i++) {
				Cards[i].Text = Morphid.Cards[i].Name;
			}
			root.Draw();
		}
	}
}