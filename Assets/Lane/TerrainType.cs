using UnityEngine;
using System.Collections.Generic;

public enum TerrainType {
    Acid = 1,
    Flooded = 2,
    Slick = 3,
    Sticky = 4
}

public static class TerrainHelper {
    public static TerrainType ChooseRandom() {
        return (TerrainType)Random.Range(1, 4);
    }
}