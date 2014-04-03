using UnityEngine;
using System.Collections;

public class MorphiumSolvent : Effect {
    public static DynamicProvider SelfDamage = () => 3 + GameState.ActiveMorphid.DamageBonus;
    public static DynamicProvider OtherDamage = () => 10 + GameState.ActiveMorphid.DamageBonus;

    public MorphiumSolvent(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return SelfDamage;
        yield return OtherDamage;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        GameState.DamageGuid(GameState.ActiveMorphid.GUID, GameState.ActiveMorphid.GUID, SelfDamage());
        GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, OtherDamage());
    }

    public override int Cost ()
    {
        return 3;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}