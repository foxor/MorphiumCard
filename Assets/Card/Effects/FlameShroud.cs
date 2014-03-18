using UnityEngine;
using System.Collections.Generic;

public class FlameShroud: Effect {
    public static DynamicProvider Healing = () => 10;

    public FlameShroud(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Healing;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        GameState.RepairGuid(guid, Healing());
        GameState.FireSetGuid(guid);
    }

    public override int Cost ()
    {
        return 4;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
