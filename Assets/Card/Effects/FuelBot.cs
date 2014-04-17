using UnityEngine;
using System;
using System.Collections.Generic;

public class FuelBot: Effect {
    public static DynamicProvider Attack = () => 3;
    public static DynamicProvider Durability = () => 15;
    public static DynamicProvider Bonus = () => 4;

    public FuelBot(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
        yield return Bonus;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion bot = GameState.SummonMinion(guid, Attack(), Durability(), Name, null);

        Action<string> onCatchFire = (string flamingGuid) => {
            if (flamingGuid == bot.GUID) {
                GameState.DestroyMinion(flamingGuid);
            }
        };

        Action<Damage> boostDamage = (Damage damage) => {
            if (damage.Source == bot.GUID) {
                Minion minionTarget = GameState.GetMinion(damage.Target);
                Morphid morphidTarget = GameState.GetMorphid(damage.Target);
                if ((minionTarget != null && minionTarget.OnFire) ||
                    (morphidTarget != null && morphidTarget.OnFire))
                {
                    damage.Magnitude += Bonus();
                }
            }
        };

        bot.OnDeath += () => {
            GameStateWatcher.OnCatchFire -= onCatchFire;
            DamageProvider.DamageBoost -= boostDamage;
        };
        
        GameStateWatcher.OnCatchFire += onCatchFire;
        DamageProvider.DamageBoost += boostDamage;
    }

    public override int Cost ()
    {
        return 6;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
