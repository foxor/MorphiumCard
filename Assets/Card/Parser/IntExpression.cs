using UnityEngine;
using System.Collections;

public class IntExpression : Expression {
    protected int value;

    public IntExpression(string value) {
        this.value = int.Parse(value);
    }

    public override int Evaluate () {
        return value;
    }
}