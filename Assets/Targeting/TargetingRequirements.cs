using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TargetingRequirements {
    public int TargetFlags;
    public TargetingType TargetingType;
    
    public TargetingRequirements (Effect effect) {
        if (effect != null) {
            this.TargetFlags = effect.Targeting;
            this.TargetingType = effect.TargetingType;
        }
        else {
            this.TargetFlags = (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Morphid;
            this.TargetingType = TargetingType.All;
        }
    }
    
    public bool HasFlag (TargetTypeFlag flag) {
        return (TargetFlags & (int)flag) > 0;
    }
    
    public bool LaneAllowed (Lane l) {
        return HasFlag(TargetTypeFlag.Lane) &&
            (!HasFlag(TargetTypeFlag.Empty) || Minion.IsDead(l.FriendlyMinion(GameState.ActiveMorphid.GUID)));
    }
    
    public bool MorphidAllowed (Morphid m) {
        return HasFlag(TargetTypeFlag.Morphid) &&
            m != null &&
            ((HasFlag(TargetTypeFlag.Friendly) && m.GUID == GameState.ActiveMorphid.GUID) ||
            (HasFlag(TargetTypeFlag.Enemy) && m.GUID != GameState.ActiveMorphid.GUID));
    }
    
    public bool MinionAllowed (Minion m) {
        return HasFlag(TargetTypeFlag.Minion) &&
            m != null &&
            ((HasFlag(TargetTypeFlag.Friendly) && m.IsFriendly(GameState.ActiveMorphid.GUID)) ||
            (HasFlag(TargetTypeFlag.Enemy) && m.IsEnemy(GameState.ActiveMorphid.GUID)));
    }
    
    public IEnumerable<string> AllTargets (string guid) {
        if (HasFlag(TargetTypeFlag.Random)) {
            TargetingRequirements PotentialTargets = new TargetingRequirements(null);
            PotentialTargets.TargetFlags = TargetFlags & (~((int)TargetTypeFlag.Random));
            PotentialTargets.TargetingType = TargetingType.All;
            IEnumerable<string> orderedTargets = PotentialTargets.AllTargets(guid).OrderBy(x => Random.Range(0f, 1f));
            if (orderedTargets.Any()) {
                yield return orderedTargets.First();
            }
            yield break;
        }
        foreach (Lane lane in GameState.Singleton.Lanes) {
            if (LaneAllowed(lane) &&
                (TargetingType == TargetingType.All || lane.GUID == guid)
            ) {
                yield return lane.GUID;
            }
            foreach (Minion minion in lane.Minions) {
                if (MinionAllowed(minion) &&
                    (TargetingType == TargetingType.All || minion.GUID == guid)
            ) {
                    yield return minion.GUID;
                }
            }
        }
        foreach (Morphid morphid in GameState.Singleton.Morphids) {
            if (MorphidAllowed(morphid) &&
                (TargetingType == TargetingType.All || morphid.GUID == guid)
            ) {
                yield return morphid.GUID;
            }
        }
    }
    
    public bool TargetAllowed (string guid) {
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