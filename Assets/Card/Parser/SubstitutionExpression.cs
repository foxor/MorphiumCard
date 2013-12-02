using UnityEngine;
using System.Collections.Generic;

public class SubstitutionExpression : Expression {
    public static Dictionary<string, object> Substitutions;

    static SubstitutionExpression() {
        Substitutions = new Dictionary<string, object>();
    }

    protected string substitution;

    public SubstitutionExpression(string substitution) {
        this.substitution = substitution;
    }

    public override int Evaluate() {
        return (int)Substitutions[substitution];
    }
}