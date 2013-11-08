using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum TargetingMode {
	Inactive,
	Transitional,
	ClickTargeting,
	DragTargeting,
}

public class Target {
	public Morphid Morphid;
	public Lane Lane;
	public Minion Minion;
	
	protected SelectionRegion EnemyMorphid;
	protected SelectionRegion FriendlyMorphid;
	protected SelectionRegion[] Lanes;
	protected SelectionRegion[] FriendlyMinions;
	protected SelectionRegion[] EnemyMinions;
	
	public bool HasSelected {
		get {
			return Morphid != null || Lane != null || Minion != null;
		}
	}
	
	public void SetTarget(Region Selected) {
		if (Selected == null || !Selected.Enabled || !typeof(SelectionRegion).IsAssignableFrom(Selected.GetType())) {
			return;
		}
		Morphid = ((SelectionRegion)Selected).Morphid;
		Lane = ((SelectionRegion)Selected).Lane;
		Minion = ((SelectionRegion)Selected).Minion;
	}
	
	public string GUID {
		get {
			return Morphid != null ? Morphid.GUID : (
				Lane != null ? Lane.GUID : (
				Minion != null ? Minion.GUID : null
			));
		}
	}
	
	public void Draw(Region root) {
		Region Top = root.Bisect(Region.Side.Top, 40);
		Region Bottom = root.Bisect(Region.Side.Bottom, 40);
		Region[] VerticalLanes = root.Split(Region.Direction.Horizontal, 3);
		Region[][] LaneSegments = VerticalLanes.Select(x => x.Split(Region.Direction.Vertical, 3)).ToArray();
		
		Region[] TopSegments = Top.Split(Region.Direction.Horizontal, 5);
		Region[] BottomSegments = Bottom.Split(Region.Direction.Horizontal, 5);
		
		EnemyMorphid = new SelectionRegion(TopSegments[2]) {
			Morphid = GameState.GetEnemy(Client.GUID)
		};
		FriendlyMorphid = new SelectionRegion(BottomSegments[2]) {
			Morphid = GameState.GetMorphid(Client.GUID),
			Text = "Yourself"
		};
		
		Lanes = new SelectionRegion[3];
		for (int i = 0; i < 3; i++) {
			Lanes[i] = new SelectionRegion(LaneSegments[i][1]) {
				Lane = GameState.GetLane(i)
			};
		}
		Lanes[0].Text = "Left";
		Lanes[1].Text = "Middle";
		Lanes[2].Text = "Right";
		
		FriendlyMinions = new SelectionRegion[3];
		for (int i = 0; i < 3; i++) {
			FriendlyMinions[i] = new SelectionRegion(LaneSegments[i][2]) {
			};
		}
		
		EnemyMinions = new SelectionRegion[3];
		for (int i = 0; i < 3; i++) {
			EnemyMinions[i] = new SelectionRegion(LaneSegments[i][0]) {
			};
		}
	}
	
	public void Update(TargetingRequirements req) {
		EnemyMorphid.Morphid = GameState.GetEnemy(Client.GUID);
		FriendlyMorphid.Morphid = GameState.GetMorphid(Client.GUID);
		for (int i = 0; i < 3; i++) {
			Lanes[i].Lane = GameState.GetLane(i);
			FriendlyMinions[i].Minion = Lanes[i].Lane.FriendlyMinion(Client.GUID);
			EnemyMinions[i].Minion = Lanes[i].Lane.EnemyMinion(Client.GUID);
			
			if (Minion.IsDead(FriendlyMinions[i].Minion)) {
				FriendlyMinions[i].Text = "";
			}
			else {
				FriendlyMinions[i].Text = FriendlyMinions[i].Minion.Attack + "/" + FriendlyMinions[i].Minion.Defense;
			}
			if (Minion.IsDead(EnemyMinions[i].Minion)) {
				EnemyMinions[i].Text = "";
			}
			else {
				EnemyMinions[i].Text = EnemyMinions[i].Minion.Attack + "/" + EnemyMinions[i].Minion.Defense;
			}
		}
		
		EnemyMorphid.Text = "Enemy morphid, " + Morphid.RemotePlayer.Health + " health";
		
		FriendlyMorphid.Enabled = req != null && (req.TargetingType == TargetingType.All || req.TargetAllowed(FriendlyMorphid.Morphid.GUID));
		EnemyMorphid.Enabled = req != null && (req.TargetingType == TargetingType.All || req.TargetAllowed(EnemyMorphid.Morphid.GUID));
		foreach (SelectionRegion lane in Lanes) {
			lane.Enabled = req != null && (req.TargetingType == TargetingType.All || req.TargetAllowed(lane.Lane.GUID));
		}
		foreach (SelectionRegion enemy in EnemyMinions) {
			enemy.Enabled = req != null && (req.TargetingType == TargetingType.All || req.MinionAllowed(enemy.Minion));
		}
		foreach (SelectionRegion friendly in FriendlyMinions) {
			friendly.Enabled = req != null && (req.TargetingType == TargetingType.All || req.MinionAllowed(friendly.Minion));
		}
	}
}