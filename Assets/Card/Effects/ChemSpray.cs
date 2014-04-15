using UnityEngine;
using System.Collections;

public class ChemSpray : Effect {
    public static DamageProvider Damage = new ActiveMorphidDamageProvider(3);

    public ChemSpray(string text) : base(text) {}
    
    protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
    {
        yield return Damage.Provider;
    }
    
    protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
    {
        yield return TargetTypeFlag.Friendly;
        yield return TargetTypeFlag.Enemy;
        yield return TargetTypeFlag.Minion;
        yield return TargetTypeFlag.Morphid;
    }
    
    public override void Apply (string guid)
    {
        Damage.Apply(guid);
        GameState.Reload(GameState.ActiveMorphid.GUID, Slot.Arm, guid);
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