using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Grouping<T> : Expr<T>
    {
        internal Expr<T> _expression;
        internal Grouping(Expr<T> expression)
        {
            _expression = expression;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this);
    }
}
