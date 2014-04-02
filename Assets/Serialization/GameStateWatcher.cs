using System;

public class GameStateWatcher {
    public static Action<Minion> OnMinionDeath = (Minion minion) => {};
    public static Action<Morphid, string, int> OnMorphidDamage = (Morphid morphid, string damagerGuid, int damage) => {};
}