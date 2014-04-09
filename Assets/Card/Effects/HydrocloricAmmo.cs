﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class HydrocloricAmmo: Effect {
    public static DynamicProvider NumCharges = () => 3;
    public static DynamicProvider DamageIncrease = () => 2;

    public HydrocloricAmmo(string text) : base(text) {}

    protected override IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return NumCharges;
        yield return DamageIncrease;
    }

    protected override IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Morphid;
    }

    public override void Apply (string guid)
    {
        Action<string, string, int> onDamage = null;
        Attachment self = new Attachment() {
            Effect = this,
            RemainingCharges = NumCharges(),
            OnDestroy = () => {
                GameStateWatcher.OnDamage -= onDamage;
                GameState.AddDamageBonus(guid, -DamageIncrease());
            }
        };
        onDamage = (string damagedGuid, string damagerGuid, int damage) => {
            if (damagerGuid == guid) {
                self.SpendCharge();
            }
        };

        GameState.AddDamageBonus(guid, DamageIncrease());
        GameStateWatcher.OnDamage += onDamage;

        GameState.Attach(guid, self, Slot.Arm);
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