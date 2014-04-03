using UnityEngine;
using System;
using System.Collections.Generic;

public class AcidImplant: Effect {
    public static DynamicProvider LaneDamage = () => 5 + GameState.ActiveMorphid.DamageBonus;

    public AcidImplant(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return LaneDamage;
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
        string enemyMorphidGuid = enemy.GUID;
        Action<Minion> onDeath;
        onDeath = (Minion minion) => {
            if (minion == target) {
                GameStateWatcher.OnMinionDeath -= onDeath;
                Lane lane = GameState.GetLane(minion);
                GameState.LaneDamage(lane.GUID, enemyMorphidGuid, guid, LaneDamage());
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
