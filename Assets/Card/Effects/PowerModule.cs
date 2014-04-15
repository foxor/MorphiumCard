using UnityEngine;
using System.Collections;

public class PowerModule : Effect {
    public static DynamicProvider Attack = () => 3;
    public static DynamicProvider Durability = () => 20;
    public static DynamicProvider Engine = () => 1;

    public PowerModule(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
        yield return Engine;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion module = GameState.SummonMinion(guid, Attack(), Durability(), Name, null);
        string originalOwner = module.MorphidGUID;
        module.OnDeath += () => {
            GameState.AddEngine(originalOwner, -Engine());
        };
        GameState.AddEngine(originalOwner, Engine());
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