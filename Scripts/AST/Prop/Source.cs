using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Source<T> : Prop<T>
    {
        internal Token _source;
        internal Expr<T> _value;

        internal Source(Token source, Expr<T> value)
        {
            _source = source;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitSourceProp(this);
    }
}
