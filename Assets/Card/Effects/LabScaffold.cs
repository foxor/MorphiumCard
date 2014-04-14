using UnityEngine;
using System;
using System.Collections.Generic;

public class LabScaffold: Effect {
    public static DynamicProvider Attack = () => 2;
    public static DynamicProvider Defense = () => 6;

    public LabScaffold(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Defense(), null);
        GameState.ChargeSet(GameState.ActiveMorphid.GUID, Slot.Head, true);
    }

    public override int Cost ()
    {
        return 2;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
