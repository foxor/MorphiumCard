using UnityEngine;
using System.Collections;

public class MinusExpression : Expression {
    protected Expression Left;
    protected Expression Right;
    
    public MinusExpression(Expression Left, Expression Right) {
        this.Left = Left;
        this.Right = Right;
    }
    
    public override float Evaluate() {
        float LeftVal = Left.Evaluate();
        float RightVal = Right.Evaluate();
        return LeftVal - RightVal;
    }
}