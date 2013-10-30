using UnityEngine;
using System.Collections;
using System.Linq;

public enum TargetTypeFlag {
	Lane = 1 << 0,
	Morphid = 1 << 1,
	Minion = 1 << 2,
	Enemy = 1 << 3,
	Friendly = 1 << 4,
	Empty = 1 << 5
}

public class TargetingRequirements {
	public int TargetFlags;
	
	public TargetingRequirements(params TargetTypeFlag[] Flags) {
		TargetFlags = 0;
		foreach (TargetTypeFlag flag in Flags) {
			TargetFlags |= (int)flag;
		}
	}
	
	public bool HasFlag(TargetTypeFlag flag) {
		return (TargetFlags | (int)flag) > 0;
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
		if (Selected == null || !typeof(SelectionArea).IsAssignableFrom(Selected.GetType())) {
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
		UI.Region[] Lanes = root.Split(UI.Region.Direction.Horizontal, 3);
		UI.Region[][] LaneSegments = Lanes.Select(x => x.Split(UI.Region.Direction.Vertical, 3)).ToArray();
		
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
		
		Morphid = GameState.GetEnemy(Client.GUID);
	}
	
	public void Update(TargetingRequirements req) {
		EnemyMorphid.Morphid = GameState.GetEnemy(Client.GUID);
		EnemyMorphid.Text = "Enemy morphid, " + Morphid.RemotePlayer.Health + " health";
	}
}
