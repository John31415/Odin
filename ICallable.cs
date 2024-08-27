using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal interface ICallable
    {
        public int Arity();
        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token);
    }

    internal class Rand : ICallable
    {
        public int Arity() => 2;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if (!(arguments[0] is long) || !(arguments[1] is long))
            {
                ErrorReporter.ThrowError("Arguments must be integers.", token);
                return null!;
            }
            Random random = new Random();
            long a = (long)arguments[0], b = (long)arguments[1];
            if (a > b) (a, b) = (b, a);
            return random.Next() % (b - a + 1) + a;
        }
    }

    internal class Log : ICallable
    {
        public int Arity() => 2;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if (!(arguments[0] is long) || !(arguments[1] is long))
            {
                ErrorReporter.ThrowError("Arguments must be integers.", token);
                return null!;
            }
            long a = (long)arguments[0], b = (long)arguments[1];
            if (a <= 0 || a == 1)
            {
                ErrorReporter.ThrowError("The first argument must be greater than 0 and not equal to 1.", token);
                return null!;
            }
            if (b <= 0)
            {
                ErrorReporter.ThrowError("The second argument must be greater than 0.", token);
                return null!;
            }
            double _base = Math.Log(a), _arg = Math.Log(b);
            return (long)(_arg / _base);
        }
    }

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

    internal class FieldOfPlayer : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if ((long)arguments[0] == gameState.TriggerPlayer)
            {
                return gameState.Field;
            }
            return gameState.OtherField;
        }
    }

    internal class DeckOfPlayer : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if ((long)arguments[0] == gameState.TriggerPlayer)
            {
                return gameState.Deck;
            }
            return gameState.OtherDeck;
        }
    }

    internal class GraveyardOfPlayer : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if ((long)arguments[0] == gameState.TriggerPlayer)
            {
                return gameState.Graveyard;
            }
            return gameState.OtherGraveyard;
        }
    }

    internal class Push : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if (!(arguments[0] is Card))
            {
                ErrorReporter.ThrowError("The argument must be a 'Card'.", token);
                return null!;
            }
            ((Lists)arguments[1]).Cards.Add((Card)arguments[0]);
            return null!;
        }
    }

    internal class SendBottom : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            if (!(arguments[0] is Card))
            {
                ErrorReporter.ThrowError("The argument must be a 'Card'.", token);
                return null!;
            }
            ((Lists)arguments[1]).Cards.Insert(0, (Card)arguments[0]);
            return null!;
        }
    }

    internal class Pop : ICallable
    {
        public int Arity() => 0;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            int length = ((Lists)arguments[0]).Cards.Count();
            if (length == 0)
            {
                ErrorReporter.ThrowError("The list is empty.", token);
                return null!;
            }
            Card card = ((Lists)arguments[0]).Cards[length - 1];
            ((Lists)arguments[0]).Cards.RemoveAt(length - 1);
            return card;
        }
    }

    internal class Remove : ICallable
    {
        public int Arity() => 1;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            int length = ((Lists)arguments[1]).Cards.Count();
            if (length == 0)
            {
                ErrorReporter.ThrowError("The list is empty.", token);
                return null!;
            }
            if (!(arguments[0] is Card))
            {
                ErrorReporter.ThrowError("The argument must be a 'Card'.", token);
                return null!;
            }
            ((Lists)arguments[1]).Cards.Remove((Card)arguments[0]);
            return null!;
        }
    }

    internal class Shuffle : ICallable
    {
        public int Arity() => 0;

        public object Call(GameState gameState, Interpreter interpreter, List<object> arguments, Token token)
        {
            int length = ((Lists)arguments[0]).Cards.Count();
            if (length == 0)
            {
                ErrorReporter.ThrowError("The list is empty.", token);
                return null!;
            }
            for (int i = 0; i < length; i++)
            {
                Random random = new Random();
                int rand = random.Next() % length;
                (((Lists)arguments[0]).Cards[i], ((Lists)arguments[0]).Cards[rand]) = (((Lists)arguments[0]).Cards[rand], ((Lists)arguments[0]).Cards[i]);
            }
            return null!;
        }
    }
}
