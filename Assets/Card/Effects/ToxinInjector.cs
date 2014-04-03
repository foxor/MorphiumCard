using UnityEngine;
using System;
using System.Collections.Generic;

public class ToxinInjector: Effect {
    public static DynamicProvider Attack = () => 1;
    public static DynamicProvider Defense = () => 1;
    public static DynamicProvider EngineDamage = () => -1;

    public ToxinInjector(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
        yield return EngineDamage;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion minion = GameState.SummonMinion(guid, Attack(), Defense(), new MinionBuilder() {
            Hazmat = true
        });
        Action<string, string, int> onDamage = (string damaged, string damager, int damage) => {
            if (damager == minion.GUID && GameState.GetMorphid(damaged) != null) {
                GameState.AddEngine(damaged, EngineDamage());
            }
        };
        Action<Minion> onDeath;
        onDeath = (Minion deadMinion) => {
            if (deadMinion == minion) {
                GameStateWatcher.OnDamage -= onDamage;
                GameStateWatcher.OnMinionDeath -= onDeath;
            }
        };
        GameStateWatcher.OnDamage += onDamage;
        GameStateWatcher.OnMinionDeath += onDeath;
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
