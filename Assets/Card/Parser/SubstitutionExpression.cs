using UnityEngine;
using System.Collections.Generic;

public delegate object Poll();

public class SubstitutionExpression : Expression {
    public static Dictionary<string, object> Substitutions;
    public static Dictionary<string, Poll> Searches;

    static SubstitutionExpression() {
        Substitutions = new Dictionary<string, object>();
        Searches = new Dictionary<string, Poll>();

        Searches[Weight.ARGUMENT_NAME] = () => GameState.ActiveMorphid.Weight;
    }

    protected string substitution;

    public SubstitutionExpression(string substitution) {
        this.substitution = substitution;
    }

    protected object Value {
        get {
            if (Searches.ContainsKey(substitution)) {
                return Searches[substitution]();
            }
            return Substitutions[substitution];
        }
    }

    public override int Evaluate() {
        return (int)Value;
    }
}