using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
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
