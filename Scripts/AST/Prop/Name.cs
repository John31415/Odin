using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Name<T> : Prop<T>
    {
        internal Token _name;
        internal Expr<T> _value;

        internal Name(Token name, Expr<T> value)
        {
            _name = name;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitNameProp(this);
    }
}
