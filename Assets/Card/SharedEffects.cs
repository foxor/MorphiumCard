using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate int DynamicProvider();

public static class SharedEffects {
    public static ApplyFn ApplyChain(params ApplyFn[] fns) {
        return (IEnumerable<string> guids) => {
            foreach (ApplyFn fn in fns) {
                fn(guids);
            }
        };
    }

    public static ApplyFn All(ApplySingleFn fn) {
        return (IEnumerable<string> guids) => {
            foreach (string guid in guids) {
                fn(guid);
            }
        };
    }

    public static ApplyFn Self(ApplySingleFn fn) {
        return (IEnumerable<string> x) => {
            fn(GameState.ActiveMorphid.GUID);
        };
    }
    
    public static ApplySingleFn Repair(DynamicProvider fn) {
        return (string guid) => {
            GameState.HealGuid(guid, fn());
        };
    }
    
    public static ApplyFn Weight(DynamicProvider fn) {
        return (IEnumerable<string> guids) => {
            GameState.ActiveMorphid.Weight += fn();
        };
    }
    
    public static ApplySingleFn Damage(DynamicProvider fn) {
        return (string guid) => {
            GameState.DamageGuid(guid, fn());
        };
    }
    
    public static ApplySingleFn Summon(DynamicProvider Attack, DynamicProvider Defense, bool Defensive) {
        return (string guid) => {
            Lane lane = GameState.GetLane(guid);
            if (lane != null) {
                lane.SpawnFriendly(new Minion(){
                    Attack = Attack(),
                    Defense = Defense(),
                    Defensive = Defensive
                });
            }
        };
    }
    
    public static ApplySingleFn Engine(DynamicProvider fn) {
        return (string guid) => {
            Morphid morphid = GameState.GetMorphid(guid);
            if (morphid != null) {
                morphid.Engine += fn();
            }
        };
    }

    public static ApplySingleFn Destroy() {
        return (string guid) => {
            GameState.RemoveMinion(guid);
        };
    }
}