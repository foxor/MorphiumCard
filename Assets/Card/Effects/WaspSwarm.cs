using UnityEngine;
using System.Collections;

public class WaspSwarm : Effect {
    public static DynamicProvider Attack = () => 2;
    public static DynamicProvider Defense = () => 6;

    public WaspSwarm(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Defense(), new MinionBuilder(){Scrounge = true});
    }

    public override int Cost ()
    {
        return 5;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
