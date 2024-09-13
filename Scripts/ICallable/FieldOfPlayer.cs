using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class FieldOfPlayer : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if (!(arguments[0] is long))
            {
                ErrorReporter.ThrowError("The argument must be an integer.", token);
                return null;
            }
            if ((long)arguments[0] == gameState.TriggerPlayer)
            {
                return gameState.Field;
            }
            return gameState.OtherField;
        }
    }
}
