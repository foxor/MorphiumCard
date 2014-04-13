using UnityEngine;
using System.Collections.Generic;

public class Flamethrower: Effect {
    public static DamageProvider Damage = new ActiveMorphidDamageProvider(6);

    public Flamethrower(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage.Provider;
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
        Damage.Apply(guid);
        GameState.FireSetGuid(guid);
    }

    public override int Cost ()
    {
        return 7;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
