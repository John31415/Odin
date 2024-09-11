using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Effect<T> : Method<T>
    {
        internal Token _effect;
        internal Prop<T> _name;
        internal Expr<T> _exprName;
        internal List<Prop<T>> _paramsv;

        internal Effect(Token effect, Expr<T> name)
        {
            _effect = effect;
            _exprName = name;
            _name = null;
            _paramsv = null;
        }

        internal Effect(Token effect, Prop<T> name, List<Prop<T>> paramsv)
        {
            _effect = effect;
            _name = name;
            _paramsv = paramsv;
            _exprName = null;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitEffectMethod(this);
    }
}
