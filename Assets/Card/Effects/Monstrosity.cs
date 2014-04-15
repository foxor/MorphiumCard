using UnityEngine;
using System.Collections.Generic;

public class Monstrosity: Effect {
    public static DynamicProvider Attack = () => GameState.ActiveMorphid.Parts;
    public static DynamicProvider Defense = () => GameState.ActiveMorphid.Parts * 3;

    public Monstrosity(string text) : base(text) {}

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
        GameState.SummonMinion(guid, Attack(), Defense(), Name, null);
        GameState.ConsumeParts(GameState.ActiveMorphid.GUID);
    }

    public override int Cost ()
    {
        return 6;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
