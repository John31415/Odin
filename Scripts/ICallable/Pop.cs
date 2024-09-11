using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Pop : ICallable
    {
        public int Arity() => 0;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            int length = ((Lists)arguments[0]).Cards.Count();
            if (length == 0)
            {
                ErrorReporter.ThrowError("The list is empty.", token);
                return null;
            }
            Card card = ((Lists)arguments[0]).Cards[length - 1];
            ((Lists)arguments[0]).Cards.RemoveAt(length - 1);
            return card;
        }
    }
}
