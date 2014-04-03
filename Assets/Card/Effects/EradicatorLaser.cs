using UnityEngine;
using System.Collections.Generic;

public class EradicatorLaser: Effect {
    public static DynamicProvider Damage = () => 10 + GameState.ActiveMorphid.DamageBonus;

    public EradicatorLaser(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.LaneDamage(guid, GameState.InactiveMorphid.GUID, GameState.ActiveMorphid.GUID, Damage());
        GameState.ChargeSet(GameState.ActiveMorphid.GUID, Slot.Chest, false);
    }

    public override int Cost ()
    {
        return 5;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
