using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DivideExpression : Expression {
    public static char DELIMITER = '/';

    protected Expression[] expressions;
    
    public DivideExpression(Expression[] expressions) {
        this.expressions = expressions;
        if (this.expressions.Length != 2) {
            Debug.Log("Don't divide more than 2 things without parenthasis");
        }
    }
    
    public override int Evaluate() {
        int First = expressions.First().Evaluate();
        int Last = expressions.Last().Evaluate();
        return First / Last;
    }
}