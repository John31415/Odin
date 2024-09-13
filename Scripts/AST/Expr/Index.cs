using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Index<T> : Expr<T>
    {
        internal Token _brace;
        internal Expr<T> _list;
        internal Expr<T> _expr;

        internal Index(Token brace, Expr<T> list, Expr<T> expr)
        {
            _brace = brace;
            _list = list;
            _expr = expr;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitIndexExpr(this);
    }
}
