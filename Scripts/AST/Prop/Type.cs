using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Type<T> : Prop<T>
    {
        internal Token _type;
        internal Expr<T> _value;

        internal Type(Token type, Expr<T> value)
        {
            _type = type;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitTypeProp(this);
    }
}
