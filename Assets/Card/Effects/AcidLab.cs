using UnityEngine;
using System.Collections;

public class AcidLab : Effect {
    public static DynamicProvider Attack = () => 0;
    public static DynamicProvider Durability = () => 15;
    
    public AcidLab(string text) : base(text) {}
    
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
        
        System.Action<TerrainQuery> checkTerrain = (TerrainQuery query) => {
            Lane lane = GameState.GetLane(lab);
            if (lane != null && query.laneGuid == lane.GUID) {
                query.found = query.terrainType == TerrainType.Acid;
            }
        };
        
        lab.OnDeath += () => {
            TerrainProvider.CheckTerrain -= checkTerrain;
        };
        TerrainProvider.CheckTerrain += checkTerrain;
    }
    
    public override int Cost ()
    {
        return 4;
    }
    
    public override TargetingType TargetingType ()
    {
        return global::TargetingType.Single;
    }
}