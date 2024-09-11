using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
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
}
