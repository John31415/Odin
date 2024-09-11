using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class HandOfPlayer : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if ((long)arguments[0] == gameState.TriggerPlayer)
            {
                return gameState.Hand;
            }
            return gameState.OtherHand;
        }
    }
}
