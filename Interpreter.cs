using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Odin
{
    internal class Interpreter : IVisitor<object>
    {
        public void Interpret(Expr<object> expr)
        {
            object value = Evaluate(expr);
            WriteLine(value);
        }

        public object VisitLiteralExpr(Literal<object> expr) => expr._value;

        public object VisitGroupingExpr(Grouping<object> expr) => Evaluate(expr._expression);

        public object VisitUnaryExpr(Unary<object> expr)
        {
            object right = Evaluate(expr._right);
            switch (expr._oper._type)
            {
                case TokenType.NOT: return !IsTrue(right);
                case TokenType.MINUS: return -(int)right;
            }
            return null!;
        }

        public object VisitBinaryExpr(Binary<object> expr)
        {
            object left = Evaluate(expr._left);
            object right = Evaluate(expr._right);
            bool band;
            int l, r;
            switch (expr._oper._type)
            {
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
                        return (int)left > (int)right;
                    }
                    break;
                case TokenType.LESS:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return (int)left < (int)right;
                    }
                    break;
                case TokenType.GREATER_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return (int)left >= (int)right;
                    }
                    break;
                case TokenType.LESS_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return (int)left <= (int)right;
                    }
                    break;
                case TokenType.LEFT_SHIFT:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l << r;
                    }
                    break;
                case TokenType.RIGHT_SHIFT:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l >> r;
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

        private object Evaluate(Expr<object> expr) => expr.Accept(this);

        private bool IsTrue(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            if (obj is int) return (int)obj == 0;
            if (obj is string) return (string)obj == "";
            return true;
        }

        private int QuickPow(int a, int b)
        {
            if (b == 0) return 0;
            int aux = 1;
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
            return true;
        }

        private (bool, int, int) CheckNumbers(object a, Token token, object b)
        {
            if ((a is string) || (b is string))
            {
                ThrowError(token, "Operands can't be strings.");
                return (false, 0, 0);
            }
            int _a, _b;
            if (a is int) _a = (int)a;
            else if ((bool)a == true) _a = 1;
            else _a = 0;
            if (b is int) _b = (int)b;
            else if ((bool)b == true) _b = 1;
            else _b = 0;
            return (true, _a, _b);
        }

        private void ThrowError(Token token, string message)
        {
            ErrorReporter.ThrowError(message, token._line, token._position, token._lineBeginning);
        }
    }
}
