﻿using UnityEngine;
using System.Collections;

public class Wasp : Effect {
    public static DynamicProvider Attack = () => 2;
    public static DynamicProvider Defense = () => 6;

    public static string NAME;

    public Wasp(string text) : base(text) {
        NAME = Name;
    }

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Defense(), Name, new MinionBuilder() {Scrounge = true});
    }

    public override int Cost ()
    {
        return 2;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
