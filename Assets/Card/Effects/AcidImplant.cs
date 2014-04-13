using UnityEngine;
using System;
using System.Collections.Generic;

public class AcidImplant: Effect {
    public static DamageProvider LaneDamage = new ActiveMorphidDamageProvider(5);

    public AcidImplant(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return LaneDamage.Provider;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        Minion target = GameState.GetMinion(guid);
        if (target == null) {
            return;
        }
        Morphid enemy = GameState.GetEnemy(target.MorphidGUID);
        if (enemy == null) {
            return;
        }
        Action<Minion> onDeath;
        onDeath = (Minion minion) => {
            if (minion == target) {
                GameStateWatcher.OnMinionDeath -= onDeath;
                Lane lane = GameState.GetLane(minion);
                LaneDamage.ApplyLaneDamage(lane.GUID);
            }
        };
        GameStateWatcher.OnMinionDeath += onDeath;
    }

    public override int Cost ()
    {
        return 5;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
