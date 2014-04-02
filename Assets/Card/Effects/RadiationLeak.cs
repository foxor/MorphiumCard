using UnityEngine;
using System.Collections;

public class RadiationLeak : Effect {
    public static DynamicProvider Damage = () => GameState.ActiveMorphid.Engine * 2;

    public RadiationLeak(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, Damage());
    }

    public override int Cost ()
    {
        return 7;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
