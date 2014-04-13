using System;

public class GameStateWatcher {
    public static Action<Minion> OnMinionDeath = (Minion minion) => {};
    public static Action<Minion> OnMinionSpawn = (Minion minion) => {};
    public static Action<Damage> OnDamage = (Damage damage) => {};
    public static Action<string> OnAttack = (string minionGuid) => {};
    public static Action<string> OnPostAttack = (string morphidGuid) => {};
    public static Action<string> OnEndTurn = (string morphidGuid) => {};
}