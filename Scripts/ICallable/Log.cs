using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Log : ICallable
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
            if (a <= 0 || a == 1)
            {
                ErrorReporter.ThrowError("The first argument must be greater than 0 and not equal to 1.", token);
                return null;
            }
            if (b <= 0)
            {
                ErrorReporter.ThrowError("The second argument must be greater than 0.", token);
                return null;
            }
            double _base = Math.Log(a), _arg = Math.Log(b);
            return (long)(_arg / _base);
        }
    }
}
