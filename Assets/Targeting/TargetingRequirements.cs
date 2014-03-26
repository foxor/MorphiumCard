using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public delegate bool TargetScanner(string guid);

public class TargetingRequirements {
    public int TargetFlags;
    public TargetingType TargetingType;
    public TargetScanner TargetScanner;
    
    public TargetingRequirements (Effect effect) {
        if (effect != null) {
            this.TargetFlags = effect.TargetFlags;
            this.TargetingType = effect.TargetingType();
            this.TargetScanner = effect.TargetScanner;
        }
        else {
            this.TargetFlags = (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Morphid;
            this.TargetingType = TargetingType.All;
            this.TargetScanner = IdentityScanner;
        }
    }

    public TargetingRequirements(int flags, TargetingType type, TargetScanner scanner = null)
    {
        this.TargetFlags = flags;
        this.TargetingType = type;
        if (scanner == null) {
            this.TargetScanner = IdentityScanner;
        }
        else {
            this.TargetScanner = scanner;
        }
    }
    
    public bool HasFlag (TargetTypeFlag flag) {
        return (TargetFlags & (int)flag) > 0;
    }
    
    public bool LaneAllowed (Lane l) {
        return HasFlag(TargetTypeFlag.Lane) &&
            TargetScanner(l.GUID) &&
            (!HasFlag(TargetTypeFlag.Empty) || l.isEmpty(GameState.ActiveMorphid.GUID));
    }
    
    public bool MorphidAllowed (Morphid m) {
        return HasFlag(TargetTypeFlag.Morphid) &&
            m != null &&
            TargetScanner(m.GUID) &&
            ((HasFlag(TargetTypeFlag.Friendly) && m.GUID == GameState.ActiveMorphid.GUID) ||
            (HasFlag(TargetTypeFlag.Enemy) && m.GUID != GameState.ActiveMorphid.GUID));
    }
    
    public bool MinionAllowed (Minion m) {
        return HasFlag(TargetTypeFlag.Minion) &&
            m != null &&
            TargetScanner(m.GUID) &&
            ((HasFlag(TargetTypeFlag.Friendly) && m.IsFriendly(GameState.ActiveMorphid.GUID)) ||
            (HasFlag(TargetTypeFlag.Enemy) && m.IsEnemy(GameState.ActiveMorphid.GUID)));
    }
    
    public IEnumerable<string> ChosenTargets (string guid) {
        if (HasFlag(TargetTypeFlag.Random)) {
            TargetingRequirements PotentialTargets = new TargetingRequirements(null);
            PotentialTargets.TargetFlags = TargetFlags & (~((int)TargetTypeFlag.Random));
            PotentialTargets.TargetingType = TargetingType.All;
            PotentialTargets.TargetScanner = TargetScanner;
            IEnumerable<string> orderedTargets = PotentialTargets.ChosenTargets(guid).OrderBy(x => Random.Range(0f, 1f));
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
    
	public IEnumerable<string> AllowedTargets () {
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
    
    protected bool IdentityScanner(string guid) {
        return true;
    }
}