using UnityEngine;
using System.Collections;

public class ScrapCannon : Effect {
    public static DynamicProvider damageMag = () => GameState.ActiveMorphid.Parts * 2 + GameState.ActiveMorphid.DamageBonus;
    public ScrapCannon (string text) : base (text){}
    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments () {
        yield return damageMag;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags () {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
        yield return TargetTypeFlag.Minion;
    }

    public override void Apply (string guid) {
        GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, damageMag());
        GameState.ConsumeParts(GameState.ActiveMorphid.GUID);
    }

    public override int Cost () {
        return 6;
    }

    public override TargetingType TargetingType () {
        return global::TargetingType.Single;
    }
}



