using UnityEngine;
using System;
using System.Collections.Generic;

public class HeavyLifter: Effect {
    public static DynamicProvider Attack = () => 8;
    public static DynamicProvider Durability = () => 20;
    public static DynamicProvider Weight = () => 2;

    public HeavyLifter(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
        yield return Weight;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion lifter = GameState.SummonMinion(guid, Attack(), Durability(), Name, null);
        Action<string> onAttack = (string minionGuid) => {
            if (minionGuid == lifter.GUID) {
                GameState.AddWeight(lifter.MorphidGUID, Weight());
            }
        };
        Action<Minion> onDeath;
        onDeath = (Minion minion) => {
            if (minion.GUID == lifter.GUID) {
                GameStateWatcher.OnAttack -= onAttack;
                GameStateWatcher.OnMinionDeath -= onDeath;
            }
        };

        GameStateWatcher.OnAttack += onAttack;
        GameStateWatcher.OnMinionDeath += onDeath;
    }

    public override int Cost ()
    {
        return 9;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
