using UnityEngine;
using System.Collections;

public class IntExpression : Expression {
    protected float value;

    public IntExpression(string value) {
        this.value = int.Parse(value);
    }

    public override float Evaluate () {
        return value;
    }
}