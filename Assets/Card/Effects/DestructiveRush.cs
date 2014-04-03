using UnityEngine;
using System.Collections.Generic;

public class DestructiveRush: Effect {
    public static DynamicProvider SelfDamage = () => 5 + GameState.ActiveMorphid.DamageBonus;
    public static DynamicProvider EnemyDamage = () => 10 + GameState.ActiveMorphid.DamageBonus;

    public DestructiveRush(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return SelfDamage;
        yield return EnemyDamage;
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
        GameState.DamageGuid(GameState.ActiveMorphid.GUID, GameState.ActiveMorphid.GUID, SelfDamage());
        if (GameState.GetMinion(guid) != null) {
            GameState.DestroyMinion(guid);
        }
        else {
            GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, EnemyDamage());
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
