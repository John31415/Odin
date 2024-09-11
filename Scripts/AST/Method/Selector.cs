using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Selector<T> : Method<T>
    {
        internal Token _selector;
        internal Prop<T> _source;
        internal Prop<T> _single;
        internal Prop<T> _predicate;

        internal Selector(Token selector, Prop<T> source, Prop<T> single, Prop<T> predicate)
        {
            _selector = selector;
            _source = source;
            _single = single;
            _predicate = predicate;
        }
        internal override T Accept(IVisitor<T> visitor) => visitor.VisitSelectorMethod(this);
    }
}
