using UnityEngine;
using System.Collections;
using System.Linq;

public class AddExpression : Expression {
    protected Expression[] expressions;

    public AddExpression(Expression[] expressions) {
        this.expressions = expressions;
    }

    public int Evaluate() {
        return expressions.Aggregate(0, (x, y) => x + y.Evaluate());
    }
}