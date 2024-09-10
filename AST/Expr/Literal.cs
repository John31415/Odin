using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Literal<T> : Expr<T>
    {
        internal object _value;
        internal Literal(object value)
        {
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this);
    }
}
