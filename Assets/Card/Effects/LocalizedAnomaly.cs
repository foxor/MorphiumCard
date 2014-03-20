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
            m.Attack ^= m.Defense;
            m.Defense ^= m.Attack;
            m.Attack ^= m.Defense;
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