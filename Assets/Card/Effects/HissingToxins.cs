using UnityEngine;
using System;
using System.Collections.Generic;

public class HissingToxins: Effect {
    public static DynamicProvider Durability = () => 10;
    public static DynamicProvider Damage = () => 2 + GameState.ActiveMorphid.DamageBonus;
    public static DynamicProvider AttackDrain = () => 1;

    public HissingToxins(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Durability;
        yield return Damage;
        yield return AttackDrain;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        Action<string, string, int> onDamage = (string morphid, string damagerGuid, int damage) => {
            if (morphid == guid && GameState.GetMinion(damagerGuid) != null) {
                GameState.DamageGuid(damagerGuid, guid, Damage());
                GameState.ReduceAttack(damagerGuid, AttackDrain());
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
