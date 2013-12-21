using UnityEngine;
using System.Collections;
using System.Linq;

public class MultiplyExpression : Expression {
    public static char DELIMITER = '*';

    protected Expression[] expressions;
    
    public MultiplyExpression(params Expression[] expressions) {
        this.expressions = expressions;
    }
    
    public override float Evaluate() {
        return expressions.Aggregate(1f, (x, y) => x * y.Evaluate());
    }
}