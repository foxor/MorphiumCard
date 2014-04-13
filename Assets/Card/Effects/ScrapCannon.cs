using UnityEngine;
using System.Collections;

public class ScrapCannon : Effect {
    public static DamageProvider damageMag = new ActiveMorphidDamageProvider(0, () => GameState.ActiveMorphid.Parts * 2);

    public ScrapCannon (string text) : base (text){}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments () {
        yield return damageMag.Provider;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags () {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid) {
        damageMag.Apply(guid);
        GameState.ConsumeParts(GameState.ActiveMorphid.GUID);
    }

    public override int Cost () {
        return 6;
    }

    public override TargetingType TargetingType () {
        return global::TargetingType.Single;
    }
}



