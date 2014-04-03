using UnityEngine;
using System.Collections.Generic;

public class HydrocloricAmmo: Effect {
    public static DynamicProvider DamageIncrease = () => 2;

    public HydrocloricAmmo(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return DamageIncrease;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        GameState.AddDamageBonus(guid, DamageIncrease());
        GameState.Attach(guid, new Attachment() {
            Effect = this,
            RemainingCharges = 3,
            OnDestroy = () => {
                GameState.AddDamageBonus(guid, -DamageIncrease());
            }
        }, Slot.Arm);
    }

    public override int Cost ()
    {
        return 5;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
