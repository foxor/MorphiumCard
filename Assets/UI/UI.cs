using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour {
	protected const int DRAG_SIZE = 5;
	protected const float DRAG_TIMER = 0.3f;
	
	public static UI Singleton;
	
	protected CardButton[] Cards;
	protected Button[] Stats;
	
	protected CardButton Selected;
	protected Target Target;
	protected TargetingRequirements CardRequirements;
	protected TargetingMode TargetingMode;
	
	protected Region root;
	
	public void Awake() {
		Singleton = this;
		
		root = new Region();
		Region CriticalStatsLayer = root.Bisect(Region.Side.Bottom, 20);
		Region CardLayer = root.Bisect(Region.Side.Bottom, 50);
		Region DrawLayer = root.Bisect(Region.Side.Bottom, 15);
		
		Region[] CriticalStatRegions = CriticalStatsLayer.Split(Region.Direction.Horizontal, 2);
		Region[] CardRegions = CardLayer.Split(Region.Direction.Horizontal, 4);
		Region[] DrawRegions = DrawLayer.Split(Region.Direction.Horizontal, 3);
		
		Stats = new Button[7];
		Cards = new CardButton[4];
		
		Stats[0] = new Button(CriticalStatRegions[0]) {
			Action = () => {}
		};
		Stats[1] = new Button(CriticalStatRegions[1]) {
			Action = Client.BoostEngine
		};
		
		Cards[0] = new CardButton(0, CardRegions[0]);
		Cards[1] = new CardButton(1, CardRegions[1]);
		Cards[2] = new CardButton(2, CardRegions[2]);
		Cards[3] = new CardButton(3, CardRegions[3]);
		
		new Button(DrawRegions[1]) {
			Text = "Draw",
			Action = Client.DrawCards
		};
	}
	
	public void Start() {
		Target = new Target();
		Target.Draw(root);
	}
	
	public Action PickupCard(int card) {
		return () => {
			if (Selected != null || TargetingMode != TargetingMode.Inactive) {
				return;
			}
			TargetingMode = TargetingMode.Transitional;
			float dx = Cards[card].Left - Input.mousePosition.x;
			float dy = Cards[card].Top - (Screen.height - Input.mousePosition.y);
			Selected = new CardButton(card, new Region() {
				Height = Cards[card].Height,
				Width = Cards[card].Width,
				Left = Cards[card].Left,
				Top = Cards[card].Top
			}) {
				Text = Cards[card].Text,
				Action = () => {}
			};
			Cards[card].Enabled = false;
			CardRequirements = new TargetingRequirements(Morphid.Cards[card].Targeting, Morphid.Cards[card].TargetingType);
			StartCoroutine(Select(dx, dy, card));
		};
	}
	
	protected IEnumerator Select(float dx, float dy, int card) {
		Region testRegion = new Region() {
			Left = (int)Input.mousePosition.x - DRAG_SIZE,
			Top = Screen.height - (int)Input.mousePosition.y - DRAG_SIZE,
			Width = DRAG_SIZE * 2,
			Height = DRAG_SIZE * 2
		};
		float startTime = DRAG_TIMER;
		
		while (true) {
			startTime -= Time.deltaTime;
			if (!Input.GetMouseButton(0)) {
				TargetingMode = TargetingMode.ClickTargeting;
				break;
			}
			if (testRegion.ContainsMouse() == null || startTime <= 0) {
				TargetingMode = TargetingMode.DragTargeting;
				break;
			}
			yield return 0;
		}
		
		bool cancel = false;
		while (
			(Input.GetMouseButton(0) && TargetingMode == TargetingMode.DragTargeting) ||
			(!Input.GetMouseButton(0) && TargetingMode == TargetingMode.ClickTargeting)
		) {
			if (Input.GetMouseButton(1)) {
				cancel = true;
				break;
			}
			yield return 0;
			Selected.Left = (int)(Input.mousePosition.x + dx);
			Selected.Top = (int)(Screen.height - Input.mousePosition.y + dy);
			Selected.invalid = true;
		}
		Target.SetTarget(root.ContainsMouse());
		if (
			CardRequirements.AllTargets(Target.GUID).Count() > 0 && 
			!cancel &&
			Morphid.Cards[card].Cost <= Morphid.LocalPlayer.Morphium
		) {
			Morphid.PlayLocalCard(card, Target.GUID);
		}
		TargetingMode = TargetingMode.Inactive;
		CardRequirements = null;
		Selected = null;
		Cards[card].Enabled = true;
	}
	
	public void OnGUI() {
		if (Morphid.LocalPlayer == null) {
			return;
		}
		GUI.enabled = GameState.IsLocalActive;
		Stats[0].Text = Morphid.LocalPlayer.Health + "/" + Morphid.MAX_HEALTH + " Health";
		Stats[1].Text = Morphid.LocalPlayer.Morphium + "/" + Morphid.MAX_MORPHIUM + " Morphium.  Boost Engine (" + Morphid.LocalPlayer.Engine + ")";
		Target.Update(CardRequirements);
		root.Draw();
		if (Selected != null) {
			Selected.Draw();
		}
		GUI.enabled = true;
	}
}