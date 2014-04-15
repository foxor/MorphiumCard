using UnityEngine;
using System;
using System.Collections.Generic;

public class SalvageSupervisor: Effect {
    public static DynamicProvider Attack = () => 8;
    public static DynamicProvider Durability = () => 26;
    public static DynamicProvider BuffAttack = () => 2;
    public static DynamicProvider BuffDurability = () => 2;

    public SalvageSupervisor(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
        yield return BuffAttack;
        yield return BuffDurability;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion supervisor = GameState.SummonMinion(guid, Attack(), Durability(), null);

        Action<Damage> damageBoost = (Damage damage) => {
            Minion friendlyScrounger = GameState.GetMinion(damage.Source);
            if (friendlyScrounger != null && friendlyScrounger.MorphidGUID == supervisor.MorphidGUID && friendlyScrounger.Scrounge && damage.Type == DamageType.Attack) {
                damage.Magnitude += BuffAttack();
            }
        };

        Action<Durability> durabilityBoost = (Durability durability) => {
            Minion friendlyScrounger = GameState.GetMinion(durability.MinionGuid);
            if (friendlyScrounger != null && friendlyScrounger.MorphidGUID == supervisor.MorphidGUID && friendlyScrounger.Scrounge) {
                durability.Magnitude += BuffDurability();
            }
        };

        supervisor.OnDeath = () => {
            DamageProvider.DamageBoost -= damageBoost;
            DurabilityProvider.DurabilityBoost -= durabilityBoost;
        };

        DamageProvider.DamageBoost += damageBoost;
        DurabilityProvider.DurabilityBoost += durabilityBoost;
    }

    public override int Cost ()
    {
        return 10;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
