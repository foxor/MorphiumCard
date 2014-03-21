﻿using UnityEngine;
using System.Collections;

public class HostileMorphium : Effect {
    public static DynamicProvider MorphiumPenalty = () => 4;

    public HostileMorphium(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return MorphiumPenalty;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        Morphid enemy = GameState.GetMorphid(guid);
        if (enemy != null) {
            GameState.AddMorphium(guid, MorphiumPenalty());
            GameState.DamageGuid(guid, enemy.Morphium);
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