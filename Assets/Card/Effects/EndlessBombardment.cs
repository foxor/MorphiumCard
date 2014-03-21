using UnityEngine;
using System.Collections;

public class EndlessBombardment : Effect {
    public static DynamicProvider AttackBuff = () => 1;

    public EndlessBombardment(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return AttackBuff;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        TargetingRequirements friendlyMinions = new TargetingRequirements(
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Minion,
            global::TargetingType.All
        );
        GameState.AddEngineSequence(guid, () => {
            foreach (string minionGuid in friendlyMinions.AllowedTargets()) {
                GameState.BuffMinion(minionGuid, AttackBuff(), 0);
            }
        });
    }

    public override int Cost ()
    {
        return 1;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
