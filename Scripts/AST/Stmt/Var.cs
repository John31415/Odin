using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Var<T> : Stmt<T>
    {
        internal Token _name;
        internal Token _type;
        internal Expr<T> _initializer;
        internal Var(Token name, Token type, Expr<T> initializer)
        {
            _name = name;
            _type = type;
            _initializer = initializer;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitVarStmt(this);
    }
}
