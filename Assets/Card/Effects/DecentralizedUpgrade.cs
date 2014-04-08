using UnityEngine;
using System.Collections.Generic;

public class DecentralizedUpgrade: Effect {
    public DynamicProvider Attack;
    public DynamicProvider Defense;

    protected int ConsumedParts = -1;

    public DecentralizedUpgrade(string text) : base(text) {
    }

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        Attack = () => ConsumedParts / 3;
        Defense = () => ConsumedParts;
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
        GameState.BuffMinion(guid, Attack(), Defense());
    }

    public override void GlobalApply()
    {
        GameState.ConsumeParts(GameState.ActiveMorphid.GUID);
    }

    public override int Cost ()
    {
        ConsumedParts = GameState.ActiveMorphid.Parts;
        return 3;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
