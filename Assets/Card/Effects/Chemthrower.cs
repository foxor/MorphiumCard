using UnityEngine;
using System.Collections;

public class Chemthrower : Effect {
    public Chemthrower(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        System.Action<Damage> damageBoost = null;
        damageBoost = (Damage damage) => {
            if (damage.Target == guid) {
                damage.Magnitude *= 2;
                DamageProvider.DamageBoost -= damageBoost;
            }
        };
        DamageProvider.DamageBoost += damageBoost;

        GameState.Reload(GameState.ActiveMorphid.GUID, Slot.Arm, guid);
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