using UnityEngine;
using System.Collections;

public class FireSupressionBot : Effect {
    public static DynamicProvider Attack = () => 5;
    public static DynamicProvider Durability = () => 5;

    public FireSupressionBot(string text) : base(text) {}

    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Attack;
        yield return Durability;
    }

    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Empty;
        yield return TargetTypeFlag.Lane;
    }

    public override void Apply (string guid)
    {
        GameState.SummonMinion(guid, Attack(), Durability(), Name, new MinionBuilder() {
            Blitz = true
        });
    }

    public override void GlobalApply() {
        TargetingRequirements req = new TargetingRequirements(
            (int)TargetTypeFlag.Friendly | (int)TargetTypeFlag.Minion | (int)TargetTypeFlag.Morphid,
            global::TargetingType.All
        );
        foreach (string guid in req.ChosenTargets(null)) {
            GameState.FireSetGuid(guid, false);
        }
    }

    public override int Cost ()
    {
        return 9;
    }

    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}