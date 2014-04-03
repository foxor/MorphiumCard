using UnityEngine;
using System.Collections.Generic;

public class Fireblast: Effect {
    public static DynamicProvider Damage = () => 10 + GameState.ActiveMorphid.DamageBonus;

    public Fireblast(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, Damage());
        GameState.FireSetGuid(GameState.ActiveMorphid.GUID);
    }

    public override int Cost ()
    {
        return 9;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
