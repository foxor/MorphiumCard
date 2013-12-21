using UnityEngine;
using System.Collections;
using System.Linq;

public class AddExpression : Expression {
    protected Expression[] expressions;

    public AddExpression(params Expression[] expressions) {
        this.expressions = expressions;
    }

    public override float Evaluate() {
        return expressions.Aggregate(0f, (x, y) => x + y.Evaluate());
    }
}