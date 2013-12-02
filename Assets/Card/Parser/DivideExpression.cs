using UnityEngine;
using System.Collections;
using System.Linq;

public class DivideExpression : Expression {
    public const string DELIMITER = "/";

    protected Expression[] expressions;
    
    public DivideExpression(Expression[] expressions) {
        this.expressions = expressions;
    }
    
    public override int Evaluate() {
        int First = expressions.Take(1).First().Evaluate();
        return expressions.Aggregate(First, (x, y) => x / y.Evaluate());
    }
}