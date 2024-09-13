using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Shuffle : ICallable
    {
        public int Arity() => 0;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            int length = ((Lists)arguments[0]).Cards.Count();
            for (int i = 0; i < length; i++)
            {
                Random random = new Random();
                int rand = random.Next() % length;
                (((Lists)arguments[0]).Cards[i], ((Lists)arguments[0]).Cards[rand]) = (((Lists)arguments[0]).Cards[rand], ((Lists)arguments[0]).Cards[i]);
            }
            return null;
        }
    }
}
