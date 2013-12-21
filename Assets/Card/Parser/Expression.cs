using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public abstract class Expression : Symbol {

    public abstract float Evaluate();

    public int EvaluateAsInt() {
        return Mathf.FloorToInt(Evaluate());
    }
}