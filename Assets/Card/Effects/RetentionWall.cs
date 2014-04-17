using UnityEngine;
using System.Collections;

public class RetentionWall : Effect {
    public static DynamicProvider Attack = () => 0;
    public static DynamicProvider Durability = () => 30;

    public RetentionWall(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Durability(), Name, new MinionBuilder() {
            Hazmat = true
        });
    }

    public override int Cost ()
    {
        return 4;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}