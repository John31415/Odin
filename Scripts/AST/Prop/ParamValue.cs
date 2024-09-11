using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class ParamValue<T> : Prop<T>
    {
        internal Token _name;
        internal Expr<T> _value;

        internal ParamValue(Token name, Expr<T> value)
        {
            _name = name;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitParamValueProp(this);
    }
}
