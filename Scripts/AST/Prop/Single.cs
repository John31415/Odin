using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Single<T> : Prop<T>
    {
        internal Token _single;
        internal Expr<T> _value;

        internal Single(Token single, Expr<T> value)
        {
            _single = single;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitSingleProp(this);
    }
}
