using UnityEngine;
using System.Collections.Generic;

public class RecklessFueling: Effect {
    public static DynamicProvider Engine = () => 2;

    protected bool hasGivenEngine = false;

    public RecklessFueling(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Engine;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        if (!hasGivenEngine) {
            GameState.AddEngine(GameState.ActiveMorphid.GUID, Engine());
            hasGivenEngine = true;
        }
        GameState.FireSetGuid(guid);
    }

    public override int Cost ()
    {
        return 4;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
