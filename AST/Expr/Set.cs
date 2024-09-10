using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Set<T> : Expr<T>
    {
        internal Expr<T> _obj;
        internal Token _name;
        internal Token _oper;
        internal Expr<T> _value;

        internal Set(Expr<T> obj, Token name, Token oper, Expr<T> value)
        {
            _obj = obj;
            _name = name;
            _oper = oper;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitSetExpr(this);
    }
}
