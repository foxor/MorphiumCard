using UnityEngine;
using System.Collections;

public class HeavyTank : Effect {
    public static DynamicProvider Attack = () => 6;
    public static DynamicProvider Defense = () => 30;

    public HeavyTank(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Defense(), Name, new MinionBuilder(){Protect = true});
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
