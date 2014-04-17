using UnityEngine;
using System.Collections;

public class FlashForge : Effect {
    public static DynamicProvider Attack = () => 3;
    public static DynamicProvider Durability = () => 18;
    
    public FlashForge(string text) : base(text) {}
    
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
        GameState.SummonMinion(guid, Attack(), Durability(), Name, null);
        foreach (Slot slot in System.Enum.GetValues(typeof(Slot))) {
            GameState.ChargeSet(GameState.ActiveMorphid.GUID, slot, true);
        }
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