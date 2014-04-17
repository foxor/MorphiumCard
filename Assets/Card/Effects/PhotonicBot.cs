using UnityEngine;
using System.Collections;

public class PhotonicBot : Effect {
    public static DynamicProvider Attack = () => 5;
    public static DynamicProvider Durability = () => 16;

    public PhotonicBot(string text) : base(text) {}

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
            LaneDamage = true
        });
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