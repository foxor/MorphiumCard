using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpreadingFires: Effect {
    public static DynamicProvider NumTargets = () => 3;

    public SpreadingFires(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return NumTargets;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply(string guid)
    {
    }

    public override void GlobalApply ()
    {
        TargetingRequirements req = new TargetingRequirements(this);
        foreach (string randomGuid in req.AllowedTargets().OrderBy(x => Random.Range(0f, 1f)).Take(3)) {
            GameState.FireSetGuid(randomGuid);
        }
    }

    public override int Cost ()
    {
        return 2;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
