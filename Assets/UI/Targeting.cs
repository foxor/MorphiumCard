﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum TargetTypeFlag {
	Lane = 1 << 0,
	Morphid = 1 << 1,
	Minion = 1 << 2,
	Enemy = 1 << 3,
	Friendly = 1 << 4,
	Empty = 1 << 5
}

public enum TargetingType {
	Single,
	All
}

public class TargetingRequirements {
	public int TargetFlags;
	public TargetingType TargetingType;
	
	public TargetingRequirements(int Flags, TargetingType TargetingType) {
		TargetFlags = 0;
		foreach (TargetTypeFlag flag in Enum.GetValues(typeof(TargetTypeFlag)).Cast<TargetTypeFlag>()) {
			if ((Flags | (int)flag) > 0 ) {
				TargetFlags |= (int)flag;
			}
		}
		this.TargetingType = TargetingType;
	}
	
	public bool HasFlag(TargetTypeFlag flag) {
		return (TargetFlags | (int)flag) > 0;
	}
	
	public bool LaneAllowed(Lane l) {
		return HasFlag(TargetTypeFlag.Lane) &&
			(!HasFlag(TargetTypeFlag.Empty) || Minion.IsDead(l.FriendlyMinion(GameState.ActiveMorphid.GUID)));
	}
	
	public bool MorphidAllowed(Morphid m) {
		return HasFlag(TargetTypeFlag.Morphid) &&
			((HasFlag(TargetTypeFlag.Friendly) && m.GUID == GameState.ActiveMorphid.GUID) ||
				(HasFlag(TargetTypeFlag.Enemy) && m.GUID != GameState.ActiveMorphid.GUID));
	}
	
	public bool MinionAllowed(Minion m) {
		return HasFlag(TargetTypeFlag.Minion) &&
			m != null &&
			((HasFlag(TargetTypeFlag.Friendly) && m.IsFriendly(GameState.ActiveMorphid.GUID)) ||
				(HasFlag(TargetTypeFlag.Enemy) && m.IsEnemy(GameState.ActiveMorphid.GUID)));
	}
	
	public IEnumerable<string> AllTargets(string guid) {
		foreach (Lane lane in GameState.Singleton.Lanes) {
			if (LaneAllowed(lane)) {
				yield return lane.GUID;
			}
			foreach (Minion minion in lane.Minions) {
				if (MinionAllowed(minion)) {
					yield return minion.GUID;
				}
			}
		}
		foreach (Morphid morphid in GameState.Singleton.Morphids) {
			if (MorphidAllowed(morphid)) {
				yield return morphid.GUID;
			}
		}
	}
	
	public bool TargetAllowed(string guid) {
		Lane lane = GameState.GetLane(guid);
		Morphid morphid = GameState.GetMorphid(guid);
		Minion minion = GameState.GetMinion(guid);
		if (lane != null) {
			return LaneAllowed(lane);
		}
		if (morphid != null) {
			return MorphidAllowed(morphid);
		}
		if (minion != null) {
			return MinionAllowed(minion);
		}
		if (TargetingType == TargetingType.All) {
			return true;
		}
		return false;
	}
}

public class SelectionArea : UI.Button {
	public Morphid Morphid;
	public Lane Lane;
	public Minion Minion;
		
	public SelectionArea(UI.Region source) : base(source) { }
}

public class Target {
	public Morphid Morphid;
	public Lane Lane;
	public Minion Minion;
	
	protected SelectionArea EnemyMorphid;
	protected SelectionArea FriendlyMorphid;
	protected SelectionArea[] Lanes;
	protected SelectionArea[] FriendlyMinions;
	protected SelectionArea[] EnemyMinions;
	
	public bool HasSelected {
		get {
			return Morphid != null || Lane != null || Minion != null;
		}
	}
	
	public void SetTarget(UI.Region Selected) {
		if (Selected == null || !Selected.Enabled || !typeof(SelectionArea).IsAssignableFrom(Selected.GetType())) {
			return;
		}
		Morphid = ((SelectionArea)Selected).Morphid;
		Lane = ((SelectionArea)Selected).Lane;
		Minion = ((SelectionArea)Selected).Minion;
	}
	
	public string GUID {
		get {
			return Morphid != null ? Morphid.GUID : (
				Lane != null ? Lane.GUID : (
				Minion != null ? Minion.GUID : null
			));
		}
	}
	
	public void Draw(UI.Region root) {
		UI.Region Top = root.Bisect(UI.Region.Side.Top, 40);
		UI.Region Bottom = root.Bisect(UI.Region.Side.Bottom, 40);
		UI.Region[] VerticalLanes = root.Split(UI.Region.Direction.Horizontal, 3);
		UI.Region[][] LaneSegments = VerticalLanes.Select(x => x.Split(UI.Region.Direction.Vertical, 3)).ToArray();
		
		UI.Region[] TopSegments = Top.Split(UI.Region.Direction.Horizontal, 5);
		UI.Region[] BottomSegments = Bottom.Split(UI.Region.Direction.Horizontal, 5);
		
		EnemyMorphid = new SelectionArea(TopSegments[2]) {
			Morphid = GameState.GetEnemy(Client.GUID)
		};
		FriendlyMorphid = new SelectionArea(BottomSegments[2]) {
			Morphid = GameState.GetMorphid(Client.GUID),
			Text = "Yourself"
		};
		
		Lanes = new SelectionArea[3];
		for (int i = 0; i < 3; i++) {
			Lanes[i] = new SelectionArea(LaneSegments[i][1]) {
				Lane = GameState.GetLane(i)
			};
		}
		Lanes[0].Text = "Left";
		Lanes[1].Text = "Middle";
		Lanes[2].Text = "Right";
		
		FriendlyMinions = new SelectionArea[3];
		for (int i = 0; i < 3; i++) {
			FriendlyMinions[i] = new SelectionArea(LaneSegments[i][2]) {
			};
		}
		
		EnemyMinions = new SelectionArea[3];
		for (int i = 0; i < 3; i++) {
			EnemyMinions[i] = new SelectionArea(LaneSegments[i][0]) {
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
		foreach (SelectionArea lane in Lanes) {
			lane.Enabled = req != null && (req.TargetingType == TargetingType.All || req.TargetAllowed(lane.Lane.GUID));
		}
		foreach (SelectionArea enemy in EnemyMinions) {
			enemy.Enabled = req != null && (req.TargetingType == TargetingType.All || req.MinionAllowed(enemy.Minion));
		}
		foreach (SelectionArea friendly in FriendlyMinions) {
			friendly.Enabled = req != null && (req.TargetingType == TargetingType.All || req.MinionAllowed(friendly.Minion));
		}
	}
}