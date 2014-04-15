using UnityEngine;
using System;
using System.Collections;

public class TerrainQuery {
    public string laneGuid;
    public TerrainType terrainType;
    public bool found;
}

public static class TerrainProvider {
    public static Action<TerrainQuery> CheckTerrain = (TerrainQuery terrainQuery) => {};

    public static TerrainType getTerrainType(string laneGuid) {
        foreach (TerrainType type in Enum.GetValues(typeof(TerrainType))) {
            TerrainQuery query = new TerrainQuery() {
                laneGuid = laneGuid,
                terrainType = type
            };
            CheckTerrain(query);
            if (query.found) {
                return type;
            }
        }
        return 0;
    }
}