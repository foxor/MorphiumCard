using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DotExpression : Expression {
    public static char DELIMITER = '.';

    protected Expression[] expressions;

    public DotExpression(Expression[] expressions) {
        this.expressions = expressions;
    }

    public override int Evaluate () {
        IEnumerable<Expression> enumerator = expressions.AsEnumerable();
        object context = ((SubstitutionExpression)enumerator.Take(1).First()).Value;
        while (enumerator.Any()) {
            string property = ((SubstitutionExpression)enumerator.Take(1).First()).substitution;
            if (context.GetType() == typeof(Minion)) {
                switch (property) {
                case "Defense":
                    return ((Minion)context).Defense;
                }
            }
        }
        Debug.Log("Dot expression failed");
        return 0;
    }
}