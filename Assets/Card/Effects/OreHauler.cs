using UnityEngine;
using System.Collections.Generic;

public class OreHauler: Effect {
    public static DynamicProvider Attack = () => 7;
    public static DynamicProvider Defense = () => 28;
    public static DynamicProvider Engine = () => 1;

    public OreHauler(string text) : base(text) {}

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
        return 7;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
