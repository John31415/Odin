using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Expression<T> : Stmt<T>
    {
        internal Expr<T> _expression;
        internal Expression(Expr<T> expression)
        {
            _expression = expression;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitExpressionStmt(this);
    }
}
