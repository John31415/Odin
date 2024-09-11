using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Params<T> : Method<T>
    {
        internal List<Prop<T>> _paramsList;

        internal Params(List<Prop<T>> paramsList)
        {
            _paramsList = paramsList;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitParamsMethod(this);
    }
}
