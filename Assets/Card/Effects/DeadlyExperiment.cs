using UnityEngine;
using System.Collections.Generic;

public class DeadlyExperiment: Effect {
    public DeadlyExperiment(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.TerrainChange(guid, TerrainHelper.ChooseRandom());
    }

    public override void GlobalApply()
    {
        GameState.Reload(GameState.ActiveMorphid.GUID, Slot.Head);
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
