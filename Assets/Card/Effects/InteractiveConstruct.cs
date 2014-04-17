using UnityEngine;
using System;
using System.Collections.Generic;

public class InteractiveConstruct: Effect {
    public static DynamicProvider Attack = () => 3;
    public static DynamicProvider Defense = () => 15;
    public static DamageProvider LaneDamage = new DamageProvider(3);

    public InteractiveConstruct(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Defense;
        yield return LaneDamage.Provider;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion construct = GameState.SummonMinion(guid, Attack(), Defense(), Name, null);

        Action<string> engineSequence = (string morphidGuid) => {
            if (morphidGuid == construct.MorphidGUID) {
                Lane lane = GameState.GetLane(construct);
                if (lane != null) {
                    LaneDamage.ApplyLaneDamage(lane.GUID, construct.GUID);
                }
            }
        };
        
        construct.OnDeath += () => {
            GameStateWatcher.OnUpgradeEngine -= engineSequence;
        };
        GameStateWatcher.OnUpgradeEngine += engineSequence;
    }

    public override int Cost ()
    {
        return 6;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}
