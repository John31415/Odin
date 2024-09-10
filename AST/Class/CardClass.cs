using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class CardClass<T> : Class<T>
    {
        internal Prop<T> _type;
        internal Prop<T> _name;
        internal Prop<T> _faction;
        internal Prop<T> _power;
        internal Prop<T> _range;
        internal Method<T> _onActivation;

        internal CardClass(Prop<T> type, Prop<T> name, Prop<T> faction, Prop<T> power, Prop<T> range, Method<T> onActivation)
        {
            _type = type;
            _name = name;
            _faction = faction;
            _power = power;
            _range = range;
            _onActivation = onActivation;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitCardClassClass(this);
    }
}
