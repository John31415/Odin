using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class OnActivation<T> : Method<T>
    {
        internal List<Method<T>> _onActBodies;

        internal OnActivation(List<Method<T>> onActBodies)
        {
            _onActBodies = onActBodies;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitOnActivationMethod(this);
    }
}
