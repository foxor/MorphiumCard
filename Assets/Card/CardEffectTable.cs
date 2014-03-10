using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class CardEffectTable {
    public static Effect RuggedSuspension(string text) {
        DynamicProvider RepairMag = () => 4;
        DynamicProvider WeightMag = () => 4;
        return new Effect(
            SharedEffects.ApplyChain(
                SharedEffects.All(SharedEffects.Repair(RepairMag)),
                SharedEffects.Weight(WeightMag)
            ),
            () => 6,
            Templating.Template(text, RepairMag, WeightMag),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Morphid,
            TargetingType.All
        );
    }
    
    public static Effect RollOver(string text) {
        DynamicProvider DamageMag = () => 3 + GameState.ActiveMorphid.Weight;
        return new Effect(
            SharedEffects.All(SharedEffects.Damage(DamageMag)),
            () => 2,
            Templating.Template(text, DamageMag),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion | (int)TargetTypeFlag.Morphid,
            TargetingType.Single
        );
    }
    
    public static Effect Pulverize(string text) {
        DynamicProvider DamageMag = () => 2 + GameState.ActiveMorphid.Weight * 2;
        return new Effect(
            SharedEffects.All(SharedEffects.Damage(DamageMag)),
            () => 7,
            Templating.Template(text, DamageMag),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion | (int)TargetTypeFlag.Morphid,
            TargetingType.Single
        );
    }
    
    public static Effect Quake(string text) {
        DynamicProvider DamageMag = () => 7;
        return new Effect(
            SharedEffects.All(SharedEffects.Damage(DamageMag)),
            () => 9,
            Templating.Template(text, DamageMag),
            (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion,
            TargetingType.All
        );
    }

    public static Effect SpareParts(string text) {
        DynamicProvider AttackMag = () => GameState.ActiveMorphid.Weight / 2;
        DynamicProvider DefenseMag = () => GameState.ActiveMorphid.Weight * 2;
        return new Effect(
            SharedEffects.All(SharedEffects.Summon(AttackMag, DefenseMag, false)),
            () => 4,
            Templating.Template(text, AttackMag, DefenseMag),
            (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Lane,
            TargetingType.Single
        );
    }

    public static Effect LoadUp(string text) {
        DynamicProvider EngineMag = () => 1;
        DynamicProvider WeightMag = () => 3;
        return new Effect(
            SharedEffects.ApplyChain(
                SharedEffects.All(SharedEffects.Engine(EngineMag)),
                SharedEffects.Weight(WeightMag)
            ),
            () => 0,
            Templating.Template(text, EngineMag, WeightMag),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Morphid,
            TargetingType.All
        );
    }

    public static Effect Salvage(string text) {
        return new Effect(
            SharedEffects.All((string guid) => {
                Minion minion = GameState.GetMinion(guid);
                if (minion != null) {
                    GameState.ActiveMorphid.Weight += minion.Defense;
                    GameState.RemoveMinion(guid);
                }
            }),
            () => 2,
            Templating.Template(text),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Minion,
            TargetingType.Single
        );
    }

    public static Effect ExoTruck(string text) {
        DynamicProvider AttackMag = () => 2;
        DynamicProvider DefenseMag = () => 7;
        return new Effect(
            SharedEffects.All(SharedEffects.Summon(AttackMag, DefenseMag, false)),
            () => 3,
            Templating.Template(text, AttackMag, DefenseMag),
            (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Lane,
            TargetingType.Single
        );
    }

    public static Effect Drones(string text) {
        DynamicProvider AttackMag = () => GameState.ActiveMorphid.Morphium;
        DynamicProvider DefenseMag = () => GameState.ActiveMorphid.Morphium * 3;
        return new Effect(
            (IEnumerable<string> x) => {
                GameState.ActiveMorphid.Research = () => {
                    Lane lane = GameState.Singleton.Lanes.Where(l => l.FriendlyMinion(GameState.ActiveMorphid.GUID) == null).OrderBy(l => UnityEngine.Random.Range(0f, 1f)).FirstOrDefault();
                    if (lane != null) {
                        lane.SpawnFriendly(new Minion(){
                            Attack = AttackMag(),
                            Defense = DefenseMag(),
                            Defensive = false
                        });
                        GameState.ActiveMorphid.Morphium = 0;
                    }
                };
            },
            () => 10,
            Templating.Template(text),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Morphid,
            TargetingType.All
        );
    }

    public static Effect AdvancedWeaponry(string text) {
        DynamicProvider DamageMag = () => GameState.ActiveMorphid.Morphium;
        return new Effect(
            (IEnumerable<string> x) => {
                GameState.ActiveMorphid.Research = () => {
                    GameState.DamageGuid(
                        GameState.GetEnemy(GameState.ActiveMorphid.GUID).GUID,
                        DamageMag()
                    );
                    GameState.ActiveMorphid.Morphium = 0;
                };
            },
            () => 10,
            Templating.Template(text),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Morphid,
            TargetingType.All
        );
    }

    public static Effect AutoTurret(string text) {
        DynamicProvider AttackMag = () => 10;
        DynamicProvider DefenseMag = () => 1;
        return new Effect(
            SharedEffects.All(SharedEffects.Summon(AttackMag, DefenseMag, true)),
            () => 5,
            Templating.Template(text, AttackMag, DefenseMag),
            (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Lane,
            TargetingType.Single
        );
    }

    public static Effect RegenerativeSuspension(string text) {
        DynamicProvider RepairMag = () => 4;
        DynamicProvider EngineMag = () => -1;
        return new Effect(
            (IEnumerable<string> guids) => {
                Action oldEngineSequence = GameState.ActiveMorphid.EngineSequence;
                GameState.ActiveMorphid.EngineSequence = () => {
                    GameState.HealGuid(GameState.ActiveMorphid.GUID, RepairMag());
                    if (oldEngineSequence != null) {
                        oldEngineSequence();
                    }
                };
                GameState.ActiveMorphid.Engine += EngineMag();
            },
            () => 7,
            Templating.Template(text, RepairMag, EngineMag),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Morphid,
            TargetingType.All
        );
    }

    public static Effect MorphiumLaser(string text) {
        DynamicProvider DamageMag = () => GameState.ActiveMorphid.Engine;
        return new Effect(
            SharedEffects.All(SharedEffects.Damage(DamageMag)),
            () => 8,
            Templating.Template(text, DamageMag),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion | (int)TargetTypeFlag.Morphid,
            TargetingType.Single
        );
    }

    public static Effect MorphiumBomb(string text) {
        DynamicProvider EngineMag = () => -4;
        return new Effect(
            SharedEffects.ApplyChain(
                SharedEffects.All(SharedEffects.Destroy()),
                SharedEffects.Self(SharedEffects.Engine(EngineMag))
            ),
            () => 8,
            Templating.Template(text, EngineMag),
            (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion,
            TargetingType.All
        );
    }

    public static Effect BladedAppendage(string text) {
        DynamicProvider DamageMag = () => 3;
        return new Effect(
            SharedEffects.All(SharedEffects.Damage(DamageMag)),
            () => 2,
            Templating.Template(text, DamageMag),
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion | (int)TargetTypeFlag.Morphid,
            TargetingType.Single
        );
    }

    public static Effect Map(string name, string text) {
        name = name.Replace(" ", "");
        return (Effect)typeof(CardEffectTable).GetMethod(name).Invoke(null, new object[]{text});
    }
}