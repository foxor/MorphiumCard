using UnityEngine;
using System.Collections.Generic;

public class DisposableWorker: Effect {
    public DisposableWorker(string text) : base(text) {}

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
        if (minion != null) {
            minion.DoAttack();
            GameState.DestroyMinion(guid);
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
