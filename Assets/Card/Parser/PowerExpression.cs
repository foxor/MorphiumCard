using UnityEngine;
using System.Collections;

public class PowerExpression : Expression {
    protected Expression Left;
    protected Expression Right;
    
    public PowerExpression(Expression Left, Expression Right) {
        this.Left = Left;
        this.Right = Right;
    }
    
    public override float Evaluate() {
        return Mathf.Pow(Left.Evaluate(), Right.Evaluate());
    }
}