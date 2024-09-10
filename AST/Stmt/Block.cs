using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Block<T> : Stmt<T>
    {
        internal List<Stmt<T>> _statements;
        internal Block(List<Stmt<T>> statements)
        {
            _statements = statements;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitBlockStmt(this);
    }
}
