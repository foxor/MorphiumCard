using UnityEngine;
using System.Collections;

public class StandTogether : Effect {
    public static DynamicProvider BuffAttack = () => 1;

    public StandTogether(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return BuffAttack;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        GameState.AddEngineSequence(guid, () => {
            TargetingRequirements req = new TargetingRequirements(
                (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Minion,
                global::TargetingType.All
            );
            foreach (string minionGuid in req.ChosenTargets(null)) {
                GameState.BuffMinion(minionGuid, BuffAttack(), 0);
            }
        });
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
