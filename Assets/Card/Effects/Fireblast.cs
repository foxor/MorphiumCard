using UnityEngine;
using System.Collections.Generic;

public class Fireblast: Effect {
    public static DamageProvider Damage = new ActiveMorphidDamageProvider(10);

    public Fireblast(string text) : base(text) {}

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
