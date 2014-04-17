using UnityEngine;
using System.Collections;

public class FragileContraption : Effect {
    public static DynamicProvider Attack = () => 9;
    public static DynamicProvider Durability = () => 1;
    
    public FragileContraption(string text) : base(text) {}
    
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
        GameState.SummonMinion(guid, Attack(), Durability(), Name, new MinionBuilder() {
            Blitz = true
        });
    }
    
    public override int Cost ()
    {
        return 5;
    }
    
    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}