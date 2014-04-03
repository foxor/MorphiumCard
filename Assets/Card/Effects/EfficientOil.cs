using UnityEngine;
using System;
using System.Collections.Generic;

public class EfficientOil: Effect {
    public static DynamicProvider Durability = () => 5;

    public EfficientOil(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Durability;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        Action<Minion> onSpawn = (Minion minion) => {
            if (minion.MorphidGUID == guid) {
                minion.Blitz = true;
            }
        };

        GameStateWatcher.OnMinionSpawn += onSpawn;
        GameState.Attach(guid, new Attachment() {
            Effect = this,
            RemainingHealth = Durability(),
            OnDestroy = () => {
                GameStateWatcher.OnMinionSpawn -= onSpawn;
            }
        }, Slot.Leg);
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
