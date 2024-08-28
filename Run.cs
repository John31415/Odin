using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Odin
{
    public class Run
    {
        public static string? Errors { get; set; }

        public List<Card>? CardsCreated { get; private set; }

        internal Dictionary<string, Method<object>> onActs { get; set; }

        public Run(GameState gameState, string input)
        {
            Errors = "";

            input = NormalizeInput.Normalize(input);

            Scanner scanner = new Scanner(input);
            List<Token> tokens = scanner.ScanTokens();

            Parser<object> parser = new Parser<object>(tokens);
            List<Class<object>> classes = parser.Parse();

            Interpreter interpreter = new Interpreter(gameState);
            Dictionary<Card, Method<object>> pairs = interpreter.CreateCards(classes);

            CardsCreated = new List<Card>();
            onActs = new Dictionary<string, Method<object>>();
            foreach(var pair in pairs)
            {
                CardsCreated.Add(pair.Key);
                onActs[pair.Key.Name] = pair.Value;
            }

            //foreach (var card in CardsCreated)
            //{
            //    WriteLine(card.Type);
            //    WriteLine(card.Name);
            //    WriteLine(card.Faction);
            //    WriteLine(card.Power);
            //    WriteLine(card.Range);
            //}
        }
    }
}
