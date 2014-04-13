using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FlameTornado: Effect {
    public static DamageProvider Damage = new ActiveMorphidDamageProvider(3);
    public static DynamicProvider NumTargets = () => 3;

    public FlameTornado(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage.Provider;
        yield return NumTargets;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply(string guid)
    {
    }

    public override void GlobalApply()
    {
        TargetingRequirements req = new TargetingRequirements(this);
        foreach (string randomGuid in req.AllowedTargets().OrderBy(x => Random.Range(0f, 1f)).Take(3)) {
            Damage.Apply(randomGuid);
        }
    }

    public override int Cost ()
    {
        return 4;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
