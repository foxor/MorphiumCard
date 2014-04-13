using UnityEngine;
using System.Collections;

public class DissolvingGrasp : Effect {
    public static DamageProvider Damage = new ActiveMorphidDamageProvider(10);
    public static DynamicProvider MorphiumDamage = () => 2;

    public DissolvingGrasp(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage.Provider;
        yield return MorphiumDamage;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        Damage.Apply(guid);
        GameState.AddMorphium(guid, -MorphiumDamage());
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