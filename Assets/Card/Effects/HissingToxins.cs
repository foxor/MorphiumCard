using UnityEngine;
using System;
using System.Collections.Generic;

public class HissingToxins: Effect {
    public static DynamicProvider Durability = () => 10;
    public static DamageProvider Damage = new ActiveMorphidDamageProvider(2);
    public static DynamicProvider AttackDrain = () => 1;

    public HissingToxins(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Durability;
        yield return Damage.Provider;
        yield return AttackDrain;
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
                Damage.Apply(damage.Source);
                GameState.ReduceAttack(damage.Source, AttackDrain());
            }
        };

        GameStateWatcher.OnDamage += onDamage;

        GameState.Attach(guid, new Attachment() {
            Effect = this,
            RemainingHealth = Durability(),
            OnDestroy = () => {
                GameStateWatcher.OnDamage -= onDamage;
            }
        }, Slot.Head);
    }

    public override int Cost ()
    {
        return 5;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
