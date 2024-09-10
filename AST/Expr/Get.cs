using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Get<T> : Expr<T>
    {
        internal Expr<T> _obj;
        internal Token _name;

        internal Get(Expr<T> obj, Token name)
        {
            _obj = obj;
            _name = name;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitGetExpr(this);
    }
}
