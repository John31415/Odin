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
        internal abstract T Accept(IVisitor<T> visitor);
    }

    internal class Binary<T> : Expr<T>
    {
        internal Expr<T> _left;
        internal Token _oper;
        internal Expr<T> _right;
        internal Binary(Expr<T> left, Token oper, Expr<T> right)
        {
            _left = left;
            _oper = oper;
            _right = right;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitBinaryExpr(this);
    }

    internal class Grouping<T> : Expr<T>
    {
        internal Expr<T> _expression;
        internal Grouping(Expr<T> expression)
        {
            _expression = expression;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this);
    }

    internal class Literal<T> : Expr<T>
    {
        internal object _value;
        internal Literal(object value)
        {
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this);
    }

    internal class Unary<T> : Expr<T>
    {
        internal Token _oper;
        internal Expr<T> _right;
        internal Unary(Token oper, Expr<T> right)
        {
            _oper = oper;
            _right = right;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitUnaryExpr(this);
    }
}