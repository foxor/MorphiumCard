using UnityEngine;
using System.Collections.Generic;

public class DecentralizedUpgrade: Effect {
    public static DynamicProvider Attack = () => GameState.ActiveMorphid.Parts;
    public static DynamicProvider Defense = () => GameState.ActiveMorphid.Parts;

    protected int ConsumedParts = -1;

    public DecentralizedUpgrade(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        if (ConsumedParts == -1) {
            ConsumedParts = GameState.ActiveMorphid.Parts;
            GameState.ConsumeParts(GameState.ActiveMorphid.GUID);
        }
        GameState.BuffMinion(guid, Attack(), Defense());
    }

    public override int Cost ()
    {
        return 3;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
