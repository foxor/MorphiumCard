using UnityEngine;
using System.Collections;
using System.Linq;

public class AddExpression : Expression {
    public const string DELIMITER = "+";

    protected Expression[] expressions;

    public AddExpression(Expression[] expressions) {
        this.expressions = expressions;
    }

    public override int Evaluate() {
        return expressions.Aggregate(0, (x, y) => x + y.Evaluate());
    }
}