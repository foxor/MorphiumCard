using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public delegate int Poll();

public class SubstitutionExpression : Expression {
    public const string MORPHID_PREFIX = "Morphid.";
    public const string CARD_PREFIX = "Card.";
    public const string MINION_PREFIX = "Minion.";

    public const string COST_SUFFIX = "Cost";

    public static Card cardContext;
    public static Minion minionContext;

    public static Dictionary<string, Poll> Searches;

    static SubstitutionExpression() {
        Searches = new Dictionary<string, Poll>();
        
        Searches[CARD_PREFIX + Damage.CSV_NAME] = () => cardContext.EffectsOfType<Damage>().First().Magnitude;
        Searches[CARD_PREFIX + Engine.CSV_NAME] = () => cardContext.EffectsOfType<Engine>().First().Magnitude;
        Searches[CARD_PREFIX + Healing.CSV_NAME] = () => cardContext.EffectsOfType<Healing>().First().Magnitude;
        Searches[CARD_PREFIX + Reflect.CSV_NAME] = () => cardContext.EffectsOfType<Reflect>().First().Magnitude;
        Searches[CARD_PREFIX + Weight.CSV_NAME] = () => cardContext.EffectsOfType<Weight>().First().Magnitude;
        Searches[CARD_PREFIX + Spawn.ATTACK_NAME] = () => cardContext.EffectsOfType<Spawn>().First().Attack;
        Searches[CARD_PREFIX + Spawn.DEFENSE_NAME] = () => cardContext.EffectsOfType<Spawn>().First().Defense;
        Searches[CARD_PREFIX + COST_SUFFIX] = () => cardContext.Cost;
        Searches[MORPHID_PREFIX + Weight.CSV_NAME] = () => GameState.ActiveMorphid.Weight;
        Searches[MINION_PREFIX + Spawn.DEFENSE_NAME] = () => minionContext.Defense;
    }

    public string substitution;

    public SubstitutionExpression(string substitution) {
        this.substitution = substitution;
    }

    public int Value {
        get {
            if (Searches.ContainsKey(substitution)) {
                return Searches[substitution]();
            }
            return 0;
        }
    }

    public override int Evaluate() {
        return Value;
    }
}