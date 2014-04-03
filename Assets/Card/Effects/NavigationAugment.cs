﻿using UnityEngine;
using System.Collections.Generic;

public class NavigationAugment: Effect {
    public NavigationAugment(string text) : base(text) {}

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
        GameState.IgnoreTerrainSet(guid, true);
        GameState.Attach(guid, new Attachment() {
            OnDestroy = () => {
                GameState.IgnoreTerrainSet(guid, false);
            },
            RemainingHealth = 8,
            Effect = this
        }, Slot.Head);
    }

    public override int Cost ()
    {
        return 2;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}