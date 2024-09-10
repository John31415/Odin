using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Range<T> : Prop<T>
    {
        internal Token _range;
        internal List<Expr<T>> _list;

        internal Range(Token range, List<Expr<T>> list)
        {
            _range = range;
            _list = list;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitRangeProp(this);
    }
}
