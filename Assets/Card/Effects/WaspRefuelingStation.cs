using UnityEngine;
using System;
using System.Collections.Generic;

public class WaspRefuelingStation: Effect {
    public static DynamicProvider Attack = () => 2;
    public static DynamicProvider Durability = () => 30;
    public static DynamicProvider BuffAttack = () => 3;
    public static DynamicProvider BuffDurability = () => 9;

    public WaspRefuelingStation(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
        yield return BuffAttack;
        yield return BuffDurability;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion station = GameState.SummonMinion(guid, Attack(), Durability(), Name, null);
        
        Action<Damage> damageBoost = (Damage damage) => {
            Minion wasp = GameState.GetMinion(damage.Source);
            if (wasp != null && wasp.MorphidGUID == station.MorphidGUID && wasp.Name == Wasp.NAME && damage.Type == DamageType.Attack) {
                damage.Magnitude += BuffAttack();
            }
        };
        
        Action<Durability> durabilityBoost = (Durability durability) => {
            Minion wasp = GameState.GetMinion(durability.MinionGuid);
            if (wasp != null && wasp.MorphidGUID == station.MorphidGUID && wasp.Name == Wasp.NAME) {
                durability.Magnitude += BuffDurability();
            }
        };
        
        station.OnDeath = () => {
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
