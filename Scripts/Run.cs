using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Odin
{
    public static class Run
    {
        public static string Errors { get; set; }

        public static List<Card> CardsCreated { get; private set; }

        internal static Dictionary<string, Method<object>> onActs { get; set; }

        internal static List<EffectClass<object>> effects { get; set; }

        public static void RunCode(string input)
        {
            Errors = "";
            effects = new List<EffectClass<object>>();

            input = NormalizeInput.Normalize(input);

            Scanner scanner = new Scanner(input);
            List<Token> tokens = scanner.ScanTokens();

            Parser<object> parser = new Parser<object>(tokens);
            List<Class<object>> classes = parser.Parse();
            WriteLine("\n\nf\n\n");

            Interpreter interpreter = new Interpreter();
            Dictionary<Card, Method<object>> pairs = interpreter.CreateCards(classes);

            CardsCreated = new List<Card>();
            onActs = new Dictionary<string, Method<object>>();
            foreach(var pair in pairs)
            {
                CardsCreated.Add(pair.Key);
                onActs[pair.Key.Name] = pair.Value;
            }
        }

        public static void RunEffect(string cardName, GameState gameState)
        {
            Errors = "";
            Interpreter interpreter = new Interpreter(gameState);
            interpreter.AplyEffect(effects, onActs[cardName]);
        }
    }
}
