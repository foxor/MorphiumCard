using UnityEngine;
using System.Collections.Generic;

public class DestructiveRush: Effect {
    public static DamageProvider SelfDamage = new ActiveMorphidDamageProvider(5);
    public static DamageProvider EnemyDamage = new ActiveMorphidDamageProvider(10);

    public DestructiveRush(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return SelfDamage.Provider;
        yield return EnemyDamage.Provider;
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
        SelfDamage.Apply(GameState.ActiveMorphid.GUID);
        if (GameState.GetMinion(guid) != null) {
            GameState.DestroyMinion(guid);
        }
        else {
            EnemyDamage.Apply(guid);
        }
    }

    public override int Cost ()
    {
        return 4;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
