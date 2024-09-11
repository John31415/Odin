using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Call<T> : Expr<T>
    {
        internal Expr<T> _callee;
        internal Token _paren;
        internal List<Expr<T>> _arguments;

        internal Call(Expr<T> callee, Token paren, List<Expr<T>> arguments)
        {
            _callee = callee;
            _paren = paren;
            _arguments = arguments;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitCallExpr(this);
    }
}
