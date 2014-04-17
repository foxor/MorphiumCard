using UnityEngine;
using System;
using System.Collections.Generic;

public class WaspFactory: Effect {
    public static DynamicProvider Attack = () => 5;
    public static DynamicProvider Defense = () => 15;
    public static DynamicProvider WaspAttack = () => 2;
    public static DynamicProvider WaspDefense = () => 6;

    public WaspFactory(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
        yield return WaspAttack;
        yield return WaspDefense;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion waspFactory = GameState.SummonMinion(guid, Attack(), Defense(), Name, null);

        TargetingRequirements req = new TargetingRequirements(
            (int)TargetTypeFlag.Random | (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Lane,
            global::TargetingType.Single
        );

        Action<string> onEndTurn = (string morphidGuid) => {
            if (morphidGuid == waspFactory.MorphidGUID) {
                foreach (string laneGuid in req.ChosenTargets(null)) {
                    GameState.SummonMinion(laneGuid, WaspAttack(), WaspDefense(), Wasp.NAME, new MinionBuilder() {
                        Scrounge = true
                    });
                }
            }
        };

        waspFactory.OnDeath += () => {
            GameStateWatcher.OnEndTurn -= onEndTurn;
        };
        GameStateWatcher.OnEndTurn += onEndTurn;
    }

    public override int Cost ()
    {
        return 8;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
