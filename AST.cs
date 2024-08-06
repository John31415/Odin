using Odin;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Console;
using static System.Math;

namespace Odin
{
    internal abstract class Expr<T>
    {
        internal abstract T accept(IVisitor<T> visitor);
    }

    internal class Binary<T> : Expr<T>
    {
        Expr<T> _left;
        Token _oper;
        Expr<T> _right;
        internal Binary(Expr<T> left, Token oper, Expr<T> right)
        {
            _left = left;
            _oper = oper;
            _right = right;
        }

        internal override T accept(IVisitor<T> visitor) => visitor.visitBinaryExpr(this);
    }

    internal class Grouping<T> : Expr<T>
    {
        Expr<T> _expression;
        internal Grouping(Expr<T> expression)
        {
            _expression = expression;
        }

        internal override T accept(IVisitor<T> visitor) => visitor.visitGroupingExpr(this);
    }

    internal class Literal<T> : Expr<T>
    {
        object _value;
        internal Literal(object value)
        {
            _value = value;
        }

        internal override T accept(IVisitor<T> visitor) => visitor.visitLiteralExpr(this);
    }

    internal class Unary<T> : Expr<T>
    {
        Token _oper;
        Expr<T> _right;
        internal Unary(Token oper, Expr<T> right)
        {
            _oper = oper;
            _right = right;
        }

        internal override T accept(IVisitor<T> visitor) => visitor.visitUnaryExpr(this);
    }
}