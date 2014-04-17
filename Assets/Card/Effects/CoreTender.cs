using UnityEngine;
using System;
using System.Collections.Generic;

public class CoreTender: Effect {
    public static DynamicProvider Attack = () => 6;
    public static DynamicProvider Defense = () => 18;
    public static DynamicProvider Engine = () => 1;

    public CoreTender(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
        yield return Engine;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Defense(), Name, null);
        GameState.AddEngine(GameState.ActiveMorphid.GUID, Engine());
    }

    public override int Cost ()
    {
        return 8;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
