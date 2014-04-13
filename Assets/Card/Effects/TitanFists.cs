using UnityEngine;
using System;
using System.Collections.Generic;

public class TitanFists: Effect {
    public static DynamicProvider Durability = () => 10;

    public TitanFists(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Durability;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        Func<string, string, int, bool, int> damageBoost = (string damagedGuid, string damagerGuid, int damage, bool realDamage) => {
            Minion damager = GameState.GetMinion(damagerGuid);
            if (damager != null && damager.MorphidGUID == guid) {
                return damage;
            }
            return 0;
        };
        DamageProvider.DamageBoost += damageBoost;
        GameState.Attach(guid, new Attachment() {
            Effect = this,
            RemainingHealth = Durability(),
            OnDestroy = () => {
                DamageProvider.DamageBoost -= damageBoost;
            }
        }, Slot.Arm);
    }

    public override int Cost ()
    {
        return 6;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
