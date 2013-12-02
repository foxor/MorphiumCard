using UnityEngine;
using System.Collections;
using System.Linq;

public class MultiplyExpression : Expression {
    public const string DELIMITER = "*";

    protected Expression[] expressions;
    
    public MultiplyExpression(Expression[] expressions) {
        this.expressions = expressions;
    }
    
    public override int Evaluate() {
        return expressions.Aggregate(1, (x, y) => x * y.Evaluate());
    }
}