using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Predicate<T> : Prop<T>
    {
        internal Token _predicate;
        internal Token _card;
        internal Expr<T> _condition;

        internal Predicate(Token predicate, Token card, Expr<T> condition)
        {
            _predicate = predicate;
            _card = card;
            _condition = condition;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPredicateProp(this);
    }
}
