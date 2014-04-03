using UnityEngine;
using System.Collections.Generic;

public class MortarStance: Effect {
    public MortarStance(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        Minion minion = GameState.GetMinion(guid);
        if (minion == null) {
            return;
        }
        int boost = minion.Attack;
        minion.InitialAttack += boost;

        GameStateWatcher.OnPostAttack += (string morphidGuid) => {
            if (morphidGuid == guid) {
                minion.InitialAttack -= boost;
            }
        };
    }

    public override void GlobalApply()
    {
        GameState.ChargeSet(GameState.ActiveMorphid.GUID, Slot.Arm, true);
        GameState.ChargeSet(GameState.ActiveMorphid.GUID, Slot.Chest, true);
        GameState.ChargeSet(GameState.ActiveMorphid.GUID, Slot.Leg, false);
    }

    public override int Cost ()
    {
        return 2;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
