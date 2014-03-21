using UnityEngine;
using System.Collections;

public class ToxicDischarge : Effect {
    public static DynamicProvider Damage = () => 6;

    public ToxicDischarge(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage;
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
        Morphid morphid = GameState.GetMorphid(guid);
        Minion minion = GameState.GetMinion(guid);
        if (morphid != null) {
            GameState.DamageGuid(guid, Damage());
        }
        if (minion != null) {
            GameState.DestroyMinion(guid);
        }
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