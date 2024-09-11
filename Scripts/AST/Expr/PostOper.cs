using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class PostOper<T> : Expr<T>
    {
        internal Expr<T> _var;
        internal Token _type;
        internal PostOper(Expr<T> var, Token type)
        {
            _var = var;
            _type = type;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPostOperExpr(this);
    }
}
