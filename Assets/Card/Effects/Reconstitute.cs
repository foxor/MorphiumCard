using UnityEngine;
using System.Collections;

public class Reconstitute : Effect {
    protected DynamicProvider Repair;
    protected int lastMorphiumCheck;

    public Reconstitute(string text) : base(text) {
        Repair = () => lastMorphiumCheck * 3;
    }

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Repair;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        GameState.RepairGuid(guid, Repair());
    }

    public override int Cost ()
    {
        lastMorphiumCheck = GameState.ActiveMorphid.Morphium;
        return lastMorphiumCheck;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}