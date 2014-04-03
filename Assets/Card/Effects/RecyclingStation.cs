using UnityEngine;
using System;
using System.Collections.Generic;

public class RecyclingStation: Effect {
    public static DynamicProvider Durability = () => 8;
    public static DynamicProvider Parts = () => 2;

    protected string attachedMorphidGuid;

    public RecyclingStation(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Durability;
        yield return Parts;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    protected void OnMinionDeath (Minion minion) {
        GameState.AddParts(attachedMorphidGuid, minion.Defense);
    }

    public override void Apply (string guid)
    {
        attachedMorphidGuid = guid;
        GameStateWatcher.OnMinionDeath += OnMinionDeath;
        Attachment recyclingStation = new Attachment() {
            OnDestroy = () => {
                GameStateWatcher.OnMinionDeath -= OnMinionDeath;
            },
            RemainingHealth = Durability(),
            Effect = this
        };
        GameState.Attach(guid, recyclingStation, Slot.Chest);
    }

    public override int Cost ()
    {
        return 7;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.All;
    }
}
