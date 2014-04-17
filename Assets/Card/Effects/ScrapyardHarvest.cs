using UnityEngine;
using System;
using System.Collections.Generic;

public class ScrapyardHarvest: Effect {
    public static DynamicProvider Attack = () => 5;
    public static DynamicProvider Durability = () => 20;
    public static DynamicProvider Parts = () => 5;

    public ScrapyardHarvest(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
        yield return Parts;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Durability(), Name, null);
        GameState.AddParts(GameState.ActiveMorphid.GUID, Parts());
    }

    public override int Cost ()
    {
        return 8;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
