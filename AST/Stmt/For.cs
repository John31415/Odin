using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class For<T> : Stmt<T>
    {
        internal Token _iter;
        internal Expr<T> _list;
        internal Stmt<T> _body;

        internal For(Token iter, Expr<T> list, Stmt<T> body)
        {
            _iter = iter;
            _list = list;
            _body = body;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitForStmt(this);
    }
}
