using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class OnActBody<T> : Method<T>
    {
        internal Method<T> _effect;
        internal Method<T> _selector;
        internal Method<T> _postAction;

        internal OnActBody(Method<T> effect, Method<T> selector, Method<T> postAction)
        {
            _effect = effect;
            _selector = selector;
            _postAction = postAction;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitOnActBodyMethod(this);
    }
}
