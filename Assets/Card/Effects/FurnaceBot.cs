using UnityEngine;
using System.Collections.Generic;

public class FurnaceBot: Effect {
    public static DynamicProvider Attack = () => 8;
    public static DynamicProvider Defense = () => 18;

    public FurnaceBot(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Defense(), Name, new MinionBuilder() {OnFire = true});
    }

    public override int Cost ()
    {
        return 5;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
