using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class Expression {
    public abstract int Evaluate();

    public static Expression Parse(string x) {
        if (x.Contains(AddExpression.DELIMITER)) {
            return new AddExpression(
                x.Split(AddExpression.DELIMITER).Select(Parse).ToArray()
                );
        }
        if (x.Contains(MultiplyExpression.DELIMITER)) {
            return new MultiplyExpression(
                x.Split(MultiplyExpression.DELIMITER).Select(Parse).ToArray()
                );
        }
        if (x.Contains(DivideExpression.DELIMITER)) {
            return new DivideExpression(
                x.Split(DivideExpression.DELIMITER).Select(Parse).ToArray()
                );
        }
        int n;
        if (int.TryParse(x, out n)) {
            return new IntExpression(x);
        }
        return SubstitutionExpression(x);
    }
}