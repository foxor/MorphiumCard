using UnityEngine;
using System.Collections;

public class EyeLasers : Effect {

    protected int lastCostCheck;
    public DynamicProvider Damage;

    public EyeLasers(string text) : base(text) {
    }

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        Damage = () => lastCostCheck * 2;
        yield return Damage;
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
        GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, Damage());
    }

    public override int Cost ()
    {
        lastCostCheck = GameState.ActiveMorphid.Morphium;
        return lastCostCheck;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}