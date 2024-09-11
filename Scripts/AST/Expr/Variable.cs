using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Variable<T> : Expr<T>
    {
        internal Token _name;
        internal Variable(Token name)
        {
            _name = name;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitVariableExpr(this);
    }
}
