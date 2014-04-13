using System;

public class GameStateWatcher {
    public static Action<Minion> OnMinionDeath = (Minion minion) => {};
    public static Action<Minion> OnMinionSpawn = (Minion minion) => {};
    public static Action<string, string, int> OnDamage = (string damagedGuid, string damagerGuid, int damage) => {};
    public static Action<string> OnPostAttack = (string morphidGuid) => {};
    public static Action<string> OnEndTurn = (string morphidGuid) => {};
}