using UnityEngine;
using System.Collections;

public class ProgressiveBrew : Effect {
    public static DynamicProvider BuffAttack = () => 2;
    public static DynamicProvider BuffDefense = () => 3;

    public ProgressiveBrew(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return BuffAttack;
        yield return BuffDefense;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        TargetingRequirements subReq = new TargetingRequirements(
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Minion,
            global::TargetingType.All
        );
        GameState.AddEngineSequence(guid, () => {
            foreach (string minionGuid in subReq.AllowedTargets()) {
                GameState.BuffMinion(minionGuid, BuffAttack(), BuffDefense());
            }
        });
    }

    public override int Cost ()
    {
        return 7;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
