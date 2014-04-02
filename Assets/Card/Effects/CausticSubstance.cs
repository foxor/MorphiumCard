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
            int damage = minion.Attack;
            TargetingRequirements allMinions = new TargetingRequirements(
                (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion,
                global::TargetingType.All
            );
            foreach (string minionGuid in allMinions.AllowedTargets()) {
                GameState.DamageGuid(minionGuid, GameState.ActiveMorphid.GUID, damage);
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
