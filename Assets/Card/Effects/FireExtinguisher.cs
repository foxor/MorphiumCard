using UnityEngine;
using System.Collections.Generic;

public class FireExtinguisher: Effect {
    public FireExtinguisher(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        GameState.FireSetGuid(guid, false);
    }

    public override int Cost ()
    {
        return 2;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }

    public override bool TargetScanner (string guid)
    {
        Minion minion = GameState.GetMinion(guid);
        Morphid morphid = GameState.GetMorphid(guid);
        if (minion != null) {
            return minion.OnFire;
        }
        if (morphid != null) {
            return morphid.OnFire;
        }
        return false;
    }
}
