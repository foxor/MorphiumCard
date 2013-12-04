using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public delegate object Poll();

public class SubstitutionExpression : Expression {
    public const string MORPHID_PREFIX = "Morphid.";
    public const string CARD_PREFIX = "Card.";
    public const string MINION_PREFIX = "Minion.";

    public static Morphid morphidContext;
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
        Searches[MORPHID_PREFIX + Weight.CSV_NAME] = () => morphidContext.Weight;
        Searches[MINION_PREFIX + Spawn.DEFENSE_NAME] = () => minionContext.Defense;
    }

    public string substitution;

    public SubstitutionExpression(string substitution) {
        this.substitution = substitution;
    }

    public object Value {
        get {
            if (Searches.ContainsKey(substitution)) {
                return Searches[substitution]();
            }
            return null;
        }
    }

    public override int Evaluate() {
        return (int)(Value ?? 0);
    }
}