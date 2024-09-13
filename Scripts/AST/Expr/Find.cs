using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Find<T> : Expr<T>
    {
        internal Token _find;
        internal Expr<T> _expr;
        internal Token _card;
        internal Expr<T> _pred;

        internal Find(Token find, Expr<T> expr, Token card, Expr<T> pred)
        {
            _find = find;
            _expr = expr;
            _card = card;
            _pred = pred;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitFindExpr(this);
    }
}
