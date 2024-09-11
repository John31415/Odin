using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Power<T> : Prop<T>
    {
        internal Token _power;
        internal Expr<T> _value;

        internal Power(Token power, Expr<T> value)
        {
            _power = power;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPowerProp(this);
    }
}
