using UnityEngine;
using System;
using System.Collections.Generic;

public class AmmoReclamation: Effect {
    public AmmoReclamation(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield break;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        string opponent = GameState.InactiveMorphid.GUID;
        Action<string, string, int> trigger;
        trigger = (string morphid, string damagerGuid, int damage) => {
            if (morphid == opponent) {
                GameStateWatcher.OnDamage -= trigger;
                GameState.AddParts(guid, damage);
            }
        };
        GameStateWatcher.OnDamage += trigger;
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
