using UnityEngine;
using System.Collections;

public class RocketSled : Effect {
    public static DynamicProvider Attack = () => 10;
    public static DynamicProvider Durability = () => 8;

    public RocketSled(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Durability(), Name, new MinionBuilder() {
            Blitz = true
        });
    }

    public override int Cost ()
    {
        return 10;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}