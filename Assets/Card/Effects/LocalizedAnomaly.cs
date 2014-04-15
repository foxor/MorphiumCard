using UnityEngine;
using System.Collections;

public class LocalizedAnomaly : Effect {

    public LocalizedAnomaly(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid)
    {
        Minion m = GameState.GetMinion(guid);
        if (m != null) {
            m.InitialAttack = m.Attack;
            m.InitialDurability = m.Durability;
            m.InitialAttack ^= m.InitialDurability;
            m.InitialDurability ^= m.InitialAttack;
            m.InitialAttack ^= m.InitialDurability;
        }
    }

    public override int Cost ()
    {
        return 3;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}