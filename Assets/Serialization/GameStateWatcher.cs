using System;

public class GameStateWatcher {
    public static Action<Minion> OnMinionDeath = (Minion minion) => {};
    public static Action<Morphid, int> OnMorphidDamage = (Morphid morphid, int damage) => {};
}