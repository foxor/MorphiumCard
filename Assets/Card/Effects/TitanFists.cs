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
        Action<Damage> damageBoost = (Damage damage) => {
            Minion damager = GameState.GetMinion(damage.Source);
            if (damager != null && damager.MorphidGUID == guid) {
                damage.Magnitude *= 2;
            }
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
