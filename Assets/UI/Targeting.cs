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

[Serializable]
public class Target {
    public SelectionRegion EnemyMorphid;
    public SelectionRegion FriendlyMorphid;
    public SelectionRegion[] Lanes;
    public SelectionRegion[] FriendlyMinions;
    public SelectionRegion[] EnemyMinions;
    
    protected Morphid Morphid;
    protected Lane Lane;
    protected Minion Minion;
    
    public bool HasSelected {
        get {
            return Morphid != null || Lane != null || Minion != null;
        }
    }
    
    public void SetTarget (SelectionRegion Selected) {
        if (Selected == null || !Selected.Enabled || !typeof(SelectionRegion).IsAssignableFrom(Selected.GetType())) {
            return;
        }
        Morphid = Selected.Morphid;
        Lane = Selected.Lane;
        Minion = Selected.Minion;
    }
    
    public string GUID {
        get {
            return Morphid != null ? Morphid.GUID : (
                Lane != null ? Lane.GUID : (
                Minion != null ? Minion.GUID : null
            ));
        }
    }
    
    public void Prepare (List<SpriteRegion> Sprites) {
        EnemyMorphid.Morphid = GameState.GetEnemy(Client.GUID);
        FriendlyMorphid.Morphid = GameState.GetMorphid (Client.GUID);

        for (int i = 0; i < 3; i++) {
            Lanes[i].Lane = GameState.GetLane(i);
        }
        
        Sprites.Add(EnemyMorphid);
        Sprites.Add(FriendlyMorphid);
        Sprites.AddRange(Lanes);
        Sprites.AddRange(EnemyMinions);
        Sprites.AddRange(FriendlyMinions);
    }
    
    public void Update (string[] guids) {
        EnemyMorphid.Morphid = GameState.GetEnemy(Client.GUID);
        FriendlyMorphid.Morphid = GameState.GetMorphid(Client.GUID);
        for (int i = 0; i < 3; i++) {
            Lanes[i].Lane = GameState.GetLane(i);
            FriendlyMinions[i].Minion = Lanes[i].Lane.FriendlyMinion(Client.GUID);
            EnemyMinions[i].Minion = Lanes[i].Lane.EnemyMinion(Client.GUID);
            
            if (Minion.IsDead(FriendlyMinions[i].Minion)) {
                FriendlyMinions[i].Text = "";
            } else {
                FriendlyMinions[i].Text = FriendlyMinions[i].Minion.CurrentAttack + "/" + FriendlyMinions[i].Minion.CurrentDurability;
            }
            if (Minion.IsDead(EnemyMinions[i].Minion)) {
                EnemyMinions[i].Text = "";
            } else {
                EnemyMinions[i].Text = EnemyMinions[i].Minion.CurrentAttack + "/" + EnemyMinions[i].Minion.CurrentDurability;
            }
        }
        
        EnemyMorphid.Text = Morphid.RemotePlayer.Health + " / " + Morphid.MAX_HEALTH;
        FriendlyMorphid.Text = Morphid.LocalPlayer.Health + " / " + Morphid.MAX_HEALTH;
        
		FriendlyMorphid.Enabled = guids != null && guids.Contains(FriendlyMorphid.Morphid.GUID);
		EnemyMorphid.Enabled = guids != null && guids.Contains(EnemyMorphid.Morphid.GUID);
        foreach (SelectionRegion lane in Lanes) {
			lane.Enabled = guids != null && guids.Contains(lane.Lane.GUID);
        }
        foreach (SelectionRegion enemy in EnemyMinions) {
			enemy.Enabled = guids != null && enemy.Minion != null && guids.Contains(enemy.Minion.GUID);
            enemy.Visible = enemy.Minion != null;
        }
        foreach (SelectionRegion friendly in FriendlyMinions) {
			friendly.Enabled = guids != null && friendly.Minion != null && guids.Contains(friendly.Minion.GUID);
            friendly.Visible = friendly.Minion != null;
        }
    }
}