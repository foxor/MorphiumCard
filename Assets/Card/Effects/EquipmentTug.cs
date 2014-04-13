using UnityEngine;
using System;
using System.Collections.Generic;

public class EquipmentTug: Effect {
    public static DynamicProvider Attack = () => 2;
    public static DynamicProvider Durability = () => 15;
    public static DynamicProvider Repair = () => 2;

    public EquipmentTug(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
        yield return Repair;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion tug = GameState.SummonMinion(guid, Attack(), Durability(), null);
        TargetingRequirements req = new TargetingRequirements(
            (int)TargetTypeFlag.Random | (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Minion, 
            global::TargetingType.Single
        );

        Action<string> onEndTurn = (string morphidGuid) => {
            if (morphidGuid == tug.MorphidGUID) {
                foreach (string minionGuid in req.ChosenTargets(null)) {
                    GameState.RepairGuid(minionGuid, Repair());
                }
            }
        };
        Action<Minion> onDeath = null;
        onDeath = (Minion minion) => {
            if (minion.GUID == tug.GUID) {
                GameStateWatcher.OnEndTurn -= onEndTurn;
                GameStateWatcher.OnMinionDeath -= onDeath;
            }
        };

        GameStateWatcher.OnEndTurn += onEndTurn;
        GameStateWatcher.OnMinionDeath += onDeath;
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
