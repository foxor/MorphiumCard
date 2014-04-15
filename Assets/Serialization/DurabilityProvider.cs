using UnityEngine;
using System;
using System.Collections.Generic;

public class Durability {
    public string MinionGuid {get;set;}
    public int Magnitude {get;set;}
}

public static class DurabilityProvider {
    public static Action<Durability> DurabilityBoost = (Durability Durability) => {};

    public static int GetDurability(Minion minion) {
        Durability durability = new Durability() {
            MinionGuid = minion.GUID,
            Magnitude = minion.InitialDurability
        };

        DurabilityBoost(durability);
        return durability.Magnitude;
    }
}