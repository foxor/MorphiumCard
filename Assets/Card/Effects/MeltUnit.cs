using UnityEngine;
using System.Collections;

public class MeltUnit : Effect {
    public static DynamicProvider Attack = () => 2;
    public static DynamicProvider Durability = () => 9;

    public MeltUnit(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        Minion meltUnit = GameState.SummonMinion(guid, Attack(), Durability(), Name, null);


        System.Action<Damage> onDamage = (Damage damage) => {
            if (damage.Source == meltUnit.GUID && damage.Type == DamageType.Attack) {
                GameState.FireSetGuid(damage.Target);
            }
        };

        meltUnit.OnDeath += () => {
            GameStateWatcher.OnDamage -= onDamage;
        };

        GameStateWatcher.OnDamage += onDamage;
    }

    public override int Cost ()
    {
        return 3;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}