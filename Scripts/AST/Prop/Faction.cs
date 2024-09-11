using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Faction<T> : Prop<T>
    {
        internal Token _faction;
        internal Expr<T> _value;

        internal Faction(Token faction, Expr<T> value)
        {
            _faction = faction;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitFactionProp(this);
    }
}
