using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class PreOper<T> : Expr<T>
    {
        internal Token _type;
        internal Expr<T> _var;
        internal PreOper(Token type, Expr<T> var)
        {
            _type = type;
            _var = var;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPreOperExpr(this);
    }
}
