using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class ParamDecl<T> : Prop<T>
    {
        internal Token _name;
        internal Token _value;

        internal ParamDecl(Token name, Token value)
        {
            _name = name;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitParamDeclProp(this);
    }
}
