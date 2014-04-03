using System;

public class GameStateWatcher {
    public static Action<Minion> OnMinionDeath = (Minion minion) => {};
    public static Action<Minion> OnMinionSpawn = (Minion minion) => {};
    public static Action<string, string, int> OnDamage = (string damagedGuid, string damagerGuid, int damage) => {};
}