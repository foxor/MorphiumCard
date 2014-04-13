using UnityEngine;
using System.Collections.Generic;

public class MiningCannon: Effect {
    public static DamageProvider Damage = new ActiveMorphidDamageProvider(5);
    public static DynamicProvider Engine = () => 1;

    public MiningCannon(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage.Provider;
        yield return Engine;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        bool before = Minion.IsDead(GameState.GetMinion(guid));
        Damage.Apply(guid);
        bool after = Minion.IsDead(GameState.GetMinion(guid));
        if (before != after) {
            GameState.AddEngine(GameState.ActiveMorphid.GUID, Engine());
        }
    }

    public override int Cost ()
    {
        return 4;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
