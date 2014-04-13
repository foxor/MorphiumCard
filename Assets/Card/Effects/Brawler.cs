using UnityEngine;
using System;
using System.Collections.Generic;

public class Brawler: Effect {
    public static DynamicProvider Attack = () => 4;
    public static DynamicProvider Defense = () => 10;

    protected DamageProvider thornsProvider;

    public Brawler(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        thornsProvider = new DamageProvider(2);
        yield return Attack;
        yield return Defense;
        yield return thornsProvider.Provider;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion brawler = GameState.SummonMinion(guid, Attack(), Defense(), null);
        Action<Damage> onDamage = (Damage damage) => {
            if (damage.Target == brawler.GUID) {
                thornsProvider.Apply(damage.Source, damage.Target);
            }
        };
        GameStateWatcher.OnDamage += onDamage;
        GameStateWatcher.OnMinionDeath += (Minion minion) => {
            if (minion.GUID == brawler.GUID) {
                GameStateWatcher.OnDamage -= onDamage;
            }
        };
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
