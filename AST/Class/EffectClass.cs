using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class EffectClass<T> : Class<T>
    {
        internal Prop<T> _name;
        internal Method<T> _params;
        internal Method<T> _action;

        internal EffectClass(Prop<T> name, Method<T> Params, Method<T> action)
        {
            _name = name;
            _params = Params;
            _action = action;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitEffectClassClass(this);
    }
}
