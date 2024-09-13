using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Count : ICallable
    {
        public int Arity() => 0;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            return (long)((Lists)arguments[0]).Cards.Count();
        }
    }
}
