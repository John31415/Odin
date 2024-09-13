using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Max : ICallable
    {
        public int Arity() => 2;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if (!(arguments[0] is long) || !(arguments[1] is long))
            {
                ErrorReporter.ThrowError("Arguments must be integers.", token);
                return null;
            }
            long a = (long)arguments[0], b = (long)arguments[1];
            return Math.Max(a, b);
        }
    }
}
