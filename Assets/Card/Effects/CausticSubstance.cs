using UnityEngine;
using System.Collections;

public class CausticSubstance : Effect {

    public CausticSubstance(string text) : base(text) {}

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
        Minion minion = GameState.GetMinion(guid);
        if (minion != null) {
            DamageProvider damage = new DamageProvider(minion.Defense);
            TargetingRequirements allMinions = new TargetingRequirements(
                (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion,
                global::TargetingType.All
            );
            foreach (string targetGuid in allMinions.AllowedTargets()) {
                damage.Apply(targetGuid, minion.GUID);
            }
        }
    }

    public override int Cost ()
    {
        return 7;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
