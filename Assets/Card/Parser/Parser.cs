using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public abstract class Symbol {}

public static class Parser {

    public delegate Symbol LexSymbol(string x);
    
    public abstract class Operator : Symbol {
        public abstract int Precidence {
            get;
        }
        
        public abstract Expression Build(Expression Left, Expression Right);
    }
    
    public class PowerOperator : Operator {
        public override Expression Build(Expression Left, Expression Right) {
            return new PowerExpression(Left, Right);
        }
        public override int Precidence {
            get {
                return 1;
            }
        }
    }
    
    public class MultiplyOperator : Operator {
        public override Expression Build(Expression Left, Expression Right) {
            return new MultiplyExpression(Left, Right);
        }
        public override int Precidence {
            get {
                return 2;
            }
        }
    }
    
    public class DivideOperator : Operator {
        public override Expression Build(Expression Left, Expression Right) {
            return new DivideExpression(Left, Right);
        }
        public override int Precidence {
            get {
                return 3;
            }
        }
    }
    
    public class AddOperator : Operator {
        public override Expression Build(Expression Left, Expression Right) {
            return new AddExpression(Left, Right);
        }
        public override int Precidence {
            get {
                return 4;
            }
        }
    }
    
    public class MinusOperator : Operator {
        public override Expression Build(Expression Left, Expression Right) {
            return new MinusExpression(Left, Right);
        }
        public override int Precidence {
            get {
                return 4;
            }
        }
    }

    public class OpenParenthasis : Symbol {}
    public class CloseParenthasis : Symbol {}

    public static Dictionary<Regex, LexSymbol> Transformations = new Dictionary<Regex, LexSymbol>() {
        {new Regex(@"^[\d]+"), x => new IntExpression(x)},
        {new Regex(@"^[A-Za-z\.]+"), x => new SubstitutionExpression(x)},
        {new Regex(@"^\^"), x => new PowerOperator()},
        {new Regex(@"^\*"), x => new MultiplyOperator()},
        {new Regex(@"^/"), x => new DivideOperator()},
        {new Regex(@"^\+"), x => new AddOperator()},
        {new Regex(@"^-"), x => new MinusOperator()},
        {new Regex(@"^\("), x => new OpenParenthasis()},
        {new Regex(@"^\)"), x => new CloseParenthasis()}
    };
    
    public static IEnumerable<Symbol> Lex(string x) {
        string last = null;
        for (int i = 0; i < x.Length; i += last.Length) {
            last = null;
            string remaining = x.Substring(i);
            foreach (Regex re in Transformations.Keys) {
                if (re.IsMatch(remaining)) {
                    last = re.Match(remaining).Value;
                    yield return Transformations[re](last);
                    break;
                }
            }
            
            if (last == null) {
                throw new ArgumentException("Unlexable remainder: \"" + remaining +"\"");
            }
        }
    }
    
    public class Context {
        public Stack<Expression> Expressions;
        public Stack<Operator> Operators;
        
        public Context() {
            Expressions = new Stack<Expression>();
            Operators = new Stack<Operator>();
        }

        public void CollapseWithPrecidence(int Precidence) {
            while (Operators.Any() && Operators.Peek().Precidence > Precidence) {
                Expression Right = Expressions.Pop();
                Expression Left = Expressions.Pop();
                Operator op = Operators.Pop();
                Expressions.Push(op.Build(Left, Right));
            }
        }
    }
    
    public static Expression Parse(string x) {
        Stack<Context> Contexts = new Stack<Context>();
        Contexts.Push(new Context());
        foreach (Symbol s in Lex(x)) {
            if (s.GetType() == typeof(OpenParenthasis)) {
                Contexts.Push(new Context());
            }
            else if (s.GetType() == typeof(CloseParenthasis)) {
                Context resolving = Contexts.Pop();
                resolving.CollapseWithPrecidence(0);
                Expression parenthetical = resolving.Expressions.Single();
                Contexts.Peek().Expressions.Push(parenthetical);
            }
            else if (typeof(Operator).IsAssignableFrom(s.GetType())) {
                Contexts.Peek().CollapseWithPrecidence(((Operator)s).Precidence);
                Contexts.Peek().Operators.Push((Operator)s);
            }
            else if (typeof(Expression).IsAssignableFrom(s.GetType())) {
                Contexts.Peek().Expressions.Push((Expression)s);
            }
            else {
                throw new ArgumentException("Cannot parse: \"" + x + "\", unrecognized: \"" + s.ToString() +"\"");
            }
        }
        Contexts.Peek().CollapseWithPrecidence(0);
        return Contexts.Single().Expressions.Single();
    }
}

public class ParserTest {
    public static void PowerTest10() {
        Expression result = Parser.Parse("(30^(1/9))^(10-1)");
        if (result.EvaluateAsInt() != 30) {
            throw new Exception();
        }
    }
    public static void PowerTest2() {
        Expression result = Parser.Parse("(30^(1/9))^(3-1)");
        if (result.EvaluateAsInt() != 2) {
            throw new Exception();
        }
    }

    public static void RunTests() {
        PowerTest10();
        PowerTest2();
    }
}