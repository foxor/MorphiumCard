using UnityEngine;
using System;
using System.Collections.Generic;

public class TitanArmor: Effect {
    public static DynamicProvider Charges = () => 5;
    public static DynamicProvider Weight = () => 6;

    public TitanArmor(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Charges;
        yield return Weight;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        Action<Damage> preventDamage = null;
        Attachment titanArmor = new Attachment() {
            Effect = this,
            RemainingCharges = Charges(),
            OnDestroy = () => {
                DamagePrevention.PreventDamage -= preventDamage;
            }
        };

        preventDamage = (Damage damage) => {
            if (damage.Target == guid && damage.Magnitude > 0) {
                titanArmor.SpendCharge();
                damage.Magnitude = 0;
            }
        };

        DamagePrevention.PreventDamage += preventDamage;
        GameState.Attach(guid, titanArmor, Slot.Chest);
        GameState.AddWeight(guid, Weight());
    }

    public override int Cost ()
    {
        return 6;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
