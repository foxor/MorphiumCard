using UnityEngine;
using System.Collections;

public class StructuralLattice : Effect {
    public static DynamicProvider AttackBuff = () => 1;
    public static DynamicProvider DefenseBuff = () => 5;

    public StructuralLattice(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return AttackBuff;
        yield return DefenseBuff;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        GameState.BuffMinion(guid, AttackBuff(), DefenseBuff());
    }

    public override int Cost ()
    {
        return 5;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}