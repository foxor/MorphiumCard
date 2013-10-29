using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {
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
		
		public virtual void Draw() {
			bool orig = GUI.enabled;
			GUI.enabled &= Enabled;
			foreach (Region child in children) {
				child.Draw();
			}
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
		
		public override void Draw () {
			base.Draw ();
			GUI.Label(ScreenRect, Text);
		}
	}
	
	
	public class Button : Region {
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
	
	protected Button[] Cards;
	protected Button[] Stats;
	
	protected Button Selected;
	protected Target Target;
	
	protected Region root;
	
	public void Awake() {
		root = new Region();
		Region CriticalStatsLayer = root.Bisect(Region.Side.Bottom, 20);
		Region CardLayer = root.Bisect(Region.Side.Bottom, 50);
		Region DrawLayer = root.Bisect(Region.Side.Bottom, 15);
		
		Region[] CriticalStatRegions = CriticalStatsLayer.Split(Region.Direction.Horizontal, 2);
		Region[] CardRegions = CardLayer.Split(Region.Direction.Horizontal, 4);
		Region[] DrawRegions = DrawLayer.Split(Region.Direction.Horizontal, 3);
		
		Stats = new Button[7];
		Cards = new Button[4];
		
		Stats[0] = new Button(CriticalStatRegions[0]) {
			Action = () => {}
		};
		Stats[1] = new Button(CriticalStatRegions[1]) {
			Action = Client.BoostEngine
		};
		
		Cards[0] = new Button(CardRegions[0]) {
			Action = Morphid.PlayLocalCardFunction(0)
		};
		Cards[1] = new Button(CardRegions[1]) {
			Action = Morphid.PlayLocalCardFunction(1)
		};
		Cards[2] = new Button(CardRegions[2]) {
			Action = Morphid.PlayLocalCardFunction(2)
		};
		Cards[3] = new Button(CardRegions[3]) {
			Action = Morphid.PlayLocalCardFunction(3)
		};
		
		new Button(DrawRegions[1]) {
			Text = "Draw",
			Action = Client.DrawCards
		};
		
		Target = new Target();
		Target.Draw(root);
	}
	
	protected Action PickupCard(int card) {
		return () => {
			float dx = Cards[card].Width / -2f, dy = Cards[card].Height / -2f;
			Selected = new Button(new Region() {
				Height = Cards[card].Height,
				Width = Cards[card].Width,
				Left = (int)(Input.mousePosition.x + dx),
				Top = (int)(Input.mousePosition.y + dy)
			}) {
				Text = Cards[card].Text,
				Action = () => {}
			};
			Cards[card].Enabled = false;
			StartCoroutine(Select(dx, dy, card));
		};
	}
	
	protected IEnumerator Select(float dx, float dy, int card) {
		while (Input.GetMouseButton(0)) {
			yield return 0;
			Selected.Left = (int)(Input.mousePosition.x + dx);
			Selected.Top = (int)(Input.mousePosition.y + dy);
		}
		Selected = null;
		Cards[card].Enabled = true;
	}
	
	public void OnGUI() {
		if (Morphid.LocalPlayer == null) {
			return;
		}
		GUI.enabled = GameState.IsLocalActive;
		for (int i = 0; i < Cards.Length; i++) {
			if (Morphid.Cards != null && Morphid.Cards[i] != null) {
				Cards[i].Text = Morphid.Cards[i].Name + " (" + Morphid.Cards[i].Cost + ")\n" + Morphid.Cards[i].Text;
			}
			else {
				Cards[i].Text = "Empty";
			}
		}
		Stats[0].Text = Morphid.LocalPlayer.Health + "/" + Morphid.MAX_HEALTH + " Health";
		Stats[1].Text = Morphid.LocalPlayer.Morphium + "/" + Morphid.MAX_MORPHIUM + " Morphium.  Boost Engine (" + Morphid.LocalPlayer.Engine + ")";
		Target.Update(null);
		root.Draw();
		if (Selected != null) {
			Selected.Draw();
		}
		GUI.enabled = true;
	}
}