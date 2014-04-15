using UnityEngine;
using System;
using System.Collections.Generic;

public class ReverseEngineer: Effect {
    public ReverseEngineer(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        Minion target = GameState.GetMinion(guid);
        if (target != null) {
            GameState.AddParts(GameState.ActiveMorphid.GUID, target.Durability);
            GameState.DestroyMinion(target.GUID);
        }
    }

    public override int Cost ()
    {
        return 7;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
