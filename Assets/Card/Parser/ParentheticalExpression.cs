using UnityEngine;
using System.Collections;

public class ParentheticalExpression : Expression {
    protected Expression parenthetical;
    
    public ParentheticalExpression(Expression parenthetical) {
        this.parenthetical = parenthetical;
    }
    
    public override float Evaluate() {
        return parenthetical.Evaluate();
    }
}