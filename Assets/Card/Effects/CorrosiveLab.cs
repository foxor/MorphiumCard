using UnityEngine;
using System.Collections;

public class CorrosiveLab : Effect {
    public static DynamicProvider Attack = () => 0;
    public static DynamicProvider Durability = () => 15;
    
    public CorrosiveLab(string text) : base(text) {}
    
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
        Minion lab = GameState.SummonMinion(guid, Attack(), Durability(), Name, null);
        
        System.Action<string> onBeginTurn = (string morhidGuid) => {
            if (morhidGuid == lab.MorphidGUID) {
                Lane myLane = GameState.GetLane(lab);
                if (myLane != null) {
                    Minion opposing = myLane.EnemyMinion(lab.MorphidGUID);
                    if (opposing != null) {
                        GameState.DestroyMinion(opposing.GUID);
                    }
                }
            }
        };
        
        lab.OnDeath += () => {
            GameStateWatcher.OnBeginTurn -= onBeginTurn;
        };
        GameStateWatcher.OnBeginTurn += onBeginTurn;
    }
    
    public override int Cost ()
    {
        return 8;
    }
    
    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}