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
    public SelectionRegion[] FriendlyMinions;
    public SelectionRegion[] EnemyMinions;
    
    public bool HasSelected {
        get {
            return Morphid != null || Lane != null || Minion != null;
        }
    }
    
    public void SetTarget (SelectionRegion Selected) {
        if (Selected == null || !Selected.Enabled || !typeof(SelectionRegion).IsAssignableFrom (Selected.GetType ())) {
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
    
    public void Draw (List<SpriteRegion> Sprites) {
        EnemyMorphid = new SelectionRegion(GameObject.FindObjectOfType<EnemyMarker>().GetComponentInChildren<MorphidMarker>().gameObject) {
            Morphid = GameState.GetEnemy(Client.GUID)
        };
        Sprites.Add(EnemyMorphid);

        FriendlyMorphid = new SelectionRegion(GameObject.FindObjectOfType<FriendlyMarker>().GetComponentInChildren<MorphidMarker>().gameObject) {
            Morphid = GameState.GetMorphid (Client.GUID)
        };
        Sprites.Add(FriendlyMorphid);
        
        Lanes = new SelectionRegion[3];
        GameObject[] Factories = GameObject.FindObjectOfType<FriendlyMarker>()
            .GetComponentsInChildren<FactoryMarker>()
            .Select(x => x.gameObject)
            .OrderBy(x => x.transform.position.x)
            .ToArray();
        for (int i = 0; i < 3; i++) {
            Lanes [i] = new SelectionRegion(Factories[i]) {
                Lane = GameState.GetLane(i)
            };
        }
        Sprites.AddRange(Lanes);
        
        FriendlyMinions = new SelectionRegion[3];
        GameObject[] FriendlySprites = GameObject.FindObjectOfType<FriendlyMarker>()
            .GetComponentsInChildren<MinionMarker>()
            .Select(x => x.gameObject)
            .OrderBy(x => x.transform.position.x)
            .ToArray();
        for (int i = 0; i < 3; i++) {
            FriendlyMinions [i] = new SelectionRegion(FriendlySprites[i]) {
            };
        }
        Sprites.AddRange(FriendlyMinions);
        
        EnemyMinions = new SelectionRegion[3];
        GameObject[] EnemySprites = GameObject.FindObjectOfType<EnemyMarker>()
            .GetComponentsInChildren<MinionMarker>()
            .Select(x => x.gameObject)
            .OrderBy(x => x.transform.position.x)
            .ToArray();
        for (int i = 0; i < 3; i++) {
            EnemyMinions [i] = new SelectionRegion(EnemySprites[i]) {
            };
        }
        Sprites.AddRange(EnemyMinions);
    }
    
    public void Update (TargetingRequirements req) {
        EnemyMorphid.Morphid = GameState.GetEnemy (Client.GUID);
        FriendlyMorphid.Morphid = GameState.GetMorphid (Client.GUID);
        for (int i = 0; i < 3; i++) {
            Lanes [i].Lane = GameState.GetLane (i);
            FriendlyMinions [i].Minion = Lanes [i].Lane.FriendlyMinion (Client.GUID);
            EnemyMinions [i].Minion = Lanes [i].Lane.EnemyMinion (Client.GUID);
            
            if (Minion.IsDead (FriendlyMinions [i].Minion)) {
                FriendlyMinions [i].Text = "";
            } else {
                FriendlyMinions [i].Text = FriendlyMinions [i].Minion.Attack + "/" + FriendlyMinions [i].Minion.Defense;
            }
            if (Minion.IsDead (EnemyMinions [i].Minion)) {
                EnemyMinions [i].Text = "";
            } else {
                EnemyMinions [i].Text = EnemyMinions [i].Minion.Attack + "/" + EnemyMinions [i].Minion.Defense;
            }
        }
        
        EnemyMorphid.Text = Morphid.RemotePlayer.Health + " / " + Morphid.MAX_HEALTH;
        FriendlyMorphid.Text = Morphid.LocalPlayer.Health + " / " + Morphid.MAX_HEALTH;
        
        FriendlyMorphid.Enabled = req != null && (req.TargetingType == TargetingType.All || req.TargetAllowed (FriendlyMorphid.Morphid.GUID));
        EnemyMorphid.Enabled = req != null && (req.TargetingType == TargetingType.All || req.TargetAllowed (EnemyMorphid.Morphid.GUID));
        foreach (SelectionRegion lane in Lanes) {
            lane.Enabled = req != null && (req.TargetingType == TargetingType.All || req.TargetAllowed (lane.Lane.GUID));
        }
        foreach (SelectionRegion enemy in EnemyMinions) {
            enemy.Enabled = req != null && (req.TargetingType == TargetingType.All || req.MinionAllowed (enemy.Minion));
            enemy.Visible = enemy.Minion != null;
        }
        foreach (SelectionRegion friendly in FriendlyMinions) {
            friendly.Enabled = req != null && (req.TargetingType == TargetingType.All || req.MinionAllowed (friendly.Minion));
            friendly.Visible = friendly.Minion != null;
        }
    }
}