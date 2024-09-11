using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal interface ICallable
    {
        int Arity();
        object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token);
    }
}
