using UnityEngine;
using System;
using System.Collections.Generic;

public class ResearchAssistant: Effect {
    public static DynamicProvider Attack = () => 3;
    public static DynamicProvider Defense = () => 10;
    public static DynamicProvider BuffAttack = () => 1;
    public static DynamicProvider BuffDefense = () => 2;
    public static DynamicProvider SpendQty = () => 3;

    public ResearchAssistant(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
        yield return BuffAttack;
        yield return BuffDefense;
        yield return SpendQty;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion assistant = GameState.SummonMinion(guid, Attack(), Defense(), Name, null);

        int spentTotal = 0;
        Action<string, int> onResearch = (string morphidGuid, int spent) => {
            if (assistant.MorphidGUID == morphidGuid) {
                spentTotal += spent;
                while (spentTotal > SpendQty()) {
                    GameState.BuffMinion(assistant.GUID, BuffAttack(), BuffDefense());
                    spentTotal -= SpendQty();
                }
            }
        };

        GameStateWatcher.OnResearch += onResearch;
        assistant.OnDeath += () => {
            GameStateWatcher.OnResearch -= onResearch;
        };
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
