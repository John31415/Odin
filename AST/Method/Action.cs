using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Action<T> : Method<T>
    {
        internal Token _targets;
        internal Token _context;
        internal List<Stmt<T>> _stmts;

        internal Action(Token targets, Token context, List<Stmt<T>> stmts)
        {
            _targets = targets;
            _context = context;
            _stmts = stmts;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitActionMethod(this);
    }
}
