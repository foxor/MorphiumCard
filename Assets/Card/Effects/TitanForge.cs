using UnityEngine;
using System;
using System.Collections.Generic;

public class TitanForge: Effect {
    public static DynamicProvider Durability = () => 20;
    public static DynamicProvider Attack = () => 1;
    public static DynamicProvider Defense = () => 3;

    public TitanForge(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Durability;
        yield return Attack;
        yield return Defense;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        TargetingRequirements req = new TargetingRequirements(
            (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Random | (int)TargetTypeFlag.Lane,
            global::TargetingType.Single
        );
        Action<string> onEndTurn = (string morphidGuid) => {
            if (morphidGuid == guid) {
                foreach (string laneGuid in req.ChosenTargets(null)) {
                    GameState.SummonMinion(laneGuid, Attack(), Defense(), Name, null);
                }
            }
        };
        GameStateWatcher.OnEndTurn += onEndTurn;
        GameState.Attach(guid, new Attachment() {
            Effect = this,
            RemainingHealth = Durability(),
            OnDestroy = () => {
                GameStateWatcher.OnEndTurn -= onEndTurn;
            }
        }, Slot.Leg);
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
