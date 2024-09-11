using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class While<T> : Stmt<T>
    {
        internal Expr<T> _condition;
        internal Stmt<T> _body;
        internal While(Expr<T> condition, Stmt<T> body)
        {
            _condition = condition;
            _body = body;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitWhileStmt(this);
    }
}
