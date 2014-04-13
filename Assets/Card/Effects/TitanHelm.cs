using UnityEngine;
using System;
using System.Collections.Generic;

public class TitanHelm: Effect {
    public static DynamicProvider Durability = () => 20;

    public TitanHelm(string text) : base(text) {}

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
        Action<Damage> onDamage = (Damage damage) => {
            if (damage.Target == guid && GameState.GetMinion(damage.Source) != null) {
                GameState.DamageGuid(new Damage() {
                    Source = guid,
                    Target = damage.Source,
                    Magnitude = damage.Magnitude
                });
            }
        };

        GameState.Attach(guid, new Attachment() {
            Effect = this,
            RemainingHealth = Durability(),
            OnDestroy = () => {
                GameStateWatcher.OnDamage -= onDamage;
            }
        }, Slot.Head);

        GameStateWatcher.OnDamage += onDamage;
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
