using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Remove : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            int length = ((Lists)arguments[1]).Cards.Count();
            if (length == 0)
            {
                ErrorReporter.ThrowError("The list is empty.", token);
                return null;
            }
            if (!(arguments[0] is Card))
            {
                ErrorReporter.ThrowError("The argument must be a 'Card'.", token);
                return null;
            }
            ((Lists)arguments[1]).Cards.Remove((Card)arguments[0]);
            return null;
        }
    }
}
