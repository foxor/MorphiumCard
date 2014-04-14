using UnityEngine;
using System;
using System.Collections.Generic;

public class Disable: Effect {
    public Disable(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        TargetingRequirements req = new TargetingRequirements(
            (int)TargetTypeFlag.Random | (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion,
            global::TargetingType.Single
        );
        GameState.SetResearch(guid, () => {
            foreach (string target in req.ChosenTargets(null)) {
                GameState.BuffMinion(target, -GameState.ActiveMorphid.Morphium, 0);
            }
        });
    }

    public override int Cost ()
    {
        return 2;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
