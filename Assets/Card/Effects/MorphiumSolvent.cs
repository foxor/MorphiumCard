using UnityEngine;
using System.Collections;

public class MorphiumSolvent : Effect {
    public static DamageProvider SelfDamage = new ActiveMorphidDamageProvider(3);
    public static DamageProvider OtherDamage = new ActiveMorphidDamageProvider(10);

    public MorphiumSolvent(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return SelfDamage.Provider;
        yield return OtherDamage.Provider;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        SelfDamage.Apply(GameState.ActiveMorphid.GUID);
        OtherDamage.Apply(guid);
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