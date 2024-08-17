using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Odin
{
    internal class Interpreter : IVisitor<object>
    {
        private Environment environment = new Environment();

        public void Interpret(List<Stmt<object>> statements)
        {
            foreach (Stmt<object> stmt in statements)
            {
                WriteLine(Execute(stmt));
            }
        }

        private object Execute(Stmt<object> stmt)
        {
            return stmt.Accept(this);
        }

        public object VisitLiteralExpr(Literal<object> expr) => expr._value;

        public object VisitGroupingExpr(Grouping<object> expr) => Evaluate(expr._expression);

        public object VisitUnaryExpr(Unary<object> expr)
        {
            object right = Evaluate(expr._right);
            switch (expr._oper._type)
            {
                case TokenType.NOT: return !IsTrue(right);
                case TokenType.MINUS: return -(long)right;
            }
            return null!;
        }

        public object VisitBinaryExpr(Binary<object> expr)
        {
            object left = Evaluate(expr._left);
            object right = Evaluate(expr._right);
            bool band;
            long l, r;
            switch (expr._oper._type)
            {
                case TokenType.AND_AND:
                    return (IsTrue(left) && IsTrue(right));
                case TokenType.OR_OR:
                    return (IsTrue(left) || IsTrue(right));
                case TokenType.EQUAL_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return IsEqual(left, right);
                    }
                    break;
                case TokenType.NOT_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return !IsEqual(left, right);
                    }
                    break;
                case TokenType.GREATER:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) > 0;
                    }
                    break;
                case TokenType.LESS:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) < 0;
                    }
                    break;
                case TokenType.GREATER_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) >= 0;
                    }
                    break;
                case TokenType.LESS_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) <= 0;
                    }
                    break;
                case TokenType.LEFT_SHIFT:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return (long)((int)l << (int)r);
                    }
                    break;
                case TokenType.RIGHT_SHIFT:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return (long)((int)l >> (int)r);
                    }
                    break;
                case TokenType.AND:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l & r;
                    }
                    break;
                case TokenType.OR:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l | r;
                    }
                    break;
                case TokenType.XOR:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l ^ r;
                    }
                    break;
                case TokenType.PLUS:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l + r;
                    }
                    break;
                case TokenType.MINUS:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l - r;
                    }
                    break;
                case TokenType.AT: return left.ToString() + right.ToString();
                case TokenType.AT_AT: return left.ToString() + " " + right.ToString();
                case TokenType.STAR:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l * r;
                    }
                    break;
                case TokenType.SLASH:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band && r != 0)
                    {
                        return l / r;
                    }
                    if (r == 0)
                    {
                        ThrowError(expr._oper, "Quotient must be a non-zero integer.");
                    }
                    break;
                case TokenType.MOD:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band && r != 0)
                    {
                        return l % r;
                    }
                    if (r == 0)
                    {
                        ThrowError(expr._oper, "Quotient must be a non-zero integer.");
                    }
                    break;
                case TokenType.EXP:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band && r >= 0)
                    {
                        return QuickPow(l, r);
                    }
                    if (r < 0)
                    {
                        ThrowError(expr._oper, "Exponent must be a non-negative integer.");
                    }
                    break;
            }
            return null!;
        }

        public object VisitExpressionStmt(Expression<object> stmt)
        {
            return Evaluate(stmt._expression);
        }

        public object VisitVarStmt(Var<object> stmt)
        {
            object value = null!;
            if (stmt._initializer != null)
            {
                value = Evaluate(stmt._initializer);
            }
            environment.Define(stmt._name._lexeme, value);
            return value;
        }

        public object VisitVariableExpr(Variable<object> expr) => environment.Get(expr._name);

        public object VisitBlockStmt(Block<object> stmt)
        {
            ExecuteBlock(stmt._statements, new Environment(environment));
            return null!;
        }

        private void ExecuteBlock(List<Stmt<object>> statements, Environment environment)
        {
            Environment previous = this.environment;
            this.environment = environment;
            foreach (Stmt<object> stmt in statements)
            {
                //Execute(stmt);
                WriteLine(Execute(stmt));
            }
            this.environment = previous;
        }

        private object Evaluate(Expr<object> expr) => expr.Accept(this);

        private bool IsTrue(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            if (obj is long) return (long)obj != 0;
            if (obj is string) return (string)obj != "";
            return true;
        }

        private long QuickPow(long a, long b)
        {
            if (b == 0) return 0;
            long aux = 1;
            while (b > 0)
            {
                if ((b % 2) == 1) aux *= a;
                a *= a;
                b >>= 1;
            }
            return aux;
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        private bool CheckComparison(object a, Token token, object b)
        {
            if (a.GetType() != b.GetType())
            {
                ThrowError(token, "Operands must be of the same type.");
                return false;
            }
            if (token._type != TokenType.EQUAL_EQUAL)
            {
                if (a is bool)
                {
                    ThrowError(token, "Operands must be integers.");
                    return false;
                }
            }
            return true;
        }

        private (bool, long, long) CheckNumbers(object a, Token token, object b)
        {
            if ((a is string) || (b is string))
            {
                ThrowError(token, "Operands can't be strings.");
                return (false, 0, 0);
            }
            long _a, _b;
            if (a is long) _a = (long)a;
            else if ((bool)a == true) _a = 1;
            else _a = 0;
            if (b is long) _b = (long)b;
            else if ((bool)b == true) _b = 1;
            else _b = 0;
            return (true, _a, _b);
        }

        private int Compare(object a, object b) => (a is long) ? ((long)a).CompareTo((long)b) : ((string)a).CompareTo((string)b);

        private void ThrowError(Token token, string message)
        {
            ErrorReporter.ThrowError(message, token._line, token._position, token._lineBeginning);
        }
    }
}
