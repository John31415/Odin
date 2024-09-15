using System.Diagnostics;
using static System.Console;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Odin
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Card> deck = new List<Card>();
            List<Card> hand = new List<Card>();
            List<Card> field = new List<Card>();
            List<Card> rip = new List<Card>();
            List<Card> otherDeck = new List<Card>();
            List<Card> otherHand = new List<Card>();
            List<Card> otherField = new List<Card>();
            List<Card> otherRip = new List<Card>();

            deck.Add(new Card(1,26,"Oro","Deck1", "Rick Sanchez", 7, "M",""));
            deck.Add(new Card(1,27,"Oro","Deck2", "Rick Sanchez", 6, "R",""));
            otherDeck.Add(new Card(2,28,"Plata","OtherDeck1", "Morty Smith", 5, "R", ""));
            otherDeck.Add(new Card(2,29,"Plata","OtherDeck2", "Morty Smith", 9, "S", ""));
            hand.Add(new Card(1,0, "Plata","Birdperson", "Rick Sanchez", 2, "R", ""));
            hand.Add(new Card(1,1, "Oro", "Hepatitis A", "Rick Sanchez", 4, "M", ""));
            hand.Add(new Card(1,2, "Plata", "Birdperson", "Rick Sanchez", 2, "R", ""));
            hand.Add(new Card(1,3, "Despeje", "Hormigas en los ojos Johnson", "Rick Sanchez", 0, "S", ""));
            hand.Add(new Card(1,4, "Clima", "Galaxy", "Rick Sanchez", 0, "R", ""));
            otherHand.Add(new Card(2,5, "Clima", "Furp Rock", "Morty Smith", 0, "R", ""));
            otherHand.Add(new Card(2,6, "Plata", "Snuffles", "Morty Smith", 3, "M", ""));
            otherHand.Add(new Card(2,7, "Plata", "Dos Hermanos", "Morty Smith", 2, "S", ""));
            otherHand.Add(new Card(2,8, "Plata", "Dos Hermanos", "Morty Smith", 2, "S", ""));
            otherHand.Add(new Card(2,9, "Aumento", "Summer", "Morty Smith", 0, "M", ""));
            otherHand.Add(new Card(2,10, "Clima", "Gramufflack", "Morty Smith", 0, "M", ""));
            otherHand.Add(new Card(2,11, "Oro", "Morty Smith Jr.", "Morty Smith", 6, "R", ""));
            otherHand.Add(new Card(2,12, "Aumento", "Jessica", "Morty Smith", 0, "S", ""));
            field.Add(new Card(1,13, "Oro", "Joyce Smith", "Rick Sanchez", 9, "M", ""));
            field.Add(new Card(1,14, "Despeje", "Ruben", "Rick Sanchez", 0, "R", ""));
            field.Add(new Card(1,15, "Plata", "Mr. Meeseeks", "Rick Sanchez", 3, "S", ""));
            otherField.Add(new Card(2,16, "Senuelo", "Rey Frijol", "Morty Smith", 0, "S", ""));
            otherField.Add(new Card(2,17, "Plata", "Mr. Poopybutthole", "Morty Smith", 1, "M", ""));
            rip.Add(new Card(1,18, "Plata", "Pickle Rick", "Rick Sanchez", 1, "M", ""));
            rip.Add(new Card(1,19, "Plata", "Mr. Meeseeks", "Rick Sanchez", 3, "R", ""));
            rip.Add(new Card(1,20, "Plata", "Mr. Meeseeks", "Rick Sanchez", 3, "R", ""));
            rip.Add(new Card(1,21, "Aumento", "Jerry Smith", "Rick Sanchez", 0, "R", ""));
            otherRip.Add(new Card(2,22, "Plata", "Dos Hermanos", "Morty Smith", 2, "R", ""));
            otherRip.Add(new Card(2,23, "Plata", "Snuffles", "Morty Smith", 3, "M", ""));
            otherRip.Add(new Card(2,24, "Despeje", "Vendedor de puertas falsas reales", "Morty Smith", 0, "R", ""));
            otherRip.Add(new Card(2,25, "Aumento", "Mr. Goldenfold", "Morty Smith", 0, "S", ""));

            List<Card> board = new List<Card>();
            board = Enumerable.Concat(board, field).ToList();
            board = Enumerable.Concat(board, otherField).ToList();

            GameState gameState = new GameState(1, 
                new Lists(board),
                new Lists(deck),
                new Lists(otherDeck),
                new Lists(hand),
                new Lists(otherHand),
                new Lists(field),
                new Lists(otherField),
                new Lists(rip),
                new Lists(otherRip));

            string userInput = "";
            userInput = "//THE CODE\r\neffect {\r\n    Name: \"Promedio\",\r\n    Action: (targets, context) => {\r\n        cant = \"0;\r\n        sum = 0;\r\n        for target in context.OtherField{\r\n            sum += target.Power;\r\n            cant++;\r\n        };\r\n        prom = sum / cant;\r\n        for target in context.OtherField{\r\n            target.Power = prom;\r\n        };\r\n    }\r\n}\r\ncard {\r\n    Type: \"Oro\",\r\n    Name: \"Juancho\",\r\n    Faction: \"Rick\"@@\"Sanchez\",\r\n    Power: 10,\r\n    Range: [\"Melee\"],\r\n    OnActivation: [\r\n        {\r\n            Effect: \"Promedio\"\r\n        }\r\n    ]\r\n}";
            Run.RunCode(userInput);
            if (!(Run.Errors == ""))
            {
                WriteLine(Run.Errors);
                return;
            }
            List<Card> cardsCreated = Run.CardsCreated;

            WriteLine(cardsCreated[0].Name);

            WriteLine();
            WriteLine("Deck");
            WriteLine();
            foreach (var i in gameState.Deck.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("OtherDeck");
            WriteLine();
            foreach (var i in gameState.OtherDeck.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("Field");
            WriteLine();
            foreach (var i in gameState.Field.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("OtherField");
            WriteLine();
            foreach (var i in gameState.OtherField.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("Hand");
            WriteLine();
            foreach (var i in gameState.Hand.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("OtherHand");
            WriteLine();
            foreach (var i in gameState.OtherHand.Cards) WriteLine(i.Name);
            WriteLine();

            Run.RunEffect(cardsCreated[0].Name, gameState);

            if (!(Run.Errors == ""))
            {
                WriteLine(Run.Errors);
                return;
            }

            WriteLine();
            WriteLine("Deck");
            WriteLine();
            foreach (var i in gameState.Deck.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("OtherDeck");
            WriteLine();
            foreach (var i in gameState.OtherDeck.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("Field");
            WriteLine();
            foreach (var i in gameState.Field.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("OtherField");
            WriteLine();
            foreach (var i in gameState.OtherField.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("Hand");
            WriteLine();
            foreach (var i in gameState.Hand.Cards) WriteLine(i.Name);
            WriteLine();
            WriteLine("OtherHand");
            WriteLine();
            foreach (var i in gameState.OtherHand.Cards) WriteLine(i.Name);
            WriteLine();

            //WriteLine();
            //WriteLine("Deck");
            //WriteLine();
            //foreach (var i in gameState.Deck.Cards) WriteLine(i.Name);
            //WriteLine();
            //WriteLine("OtherDeck");
            //WriteLine();
            //foreach (var i in gameState.OtherDeck.Cards) WriteLine(i.Name);
            //WriteLine();
            //WriteLine("Hand");
            //WriteLine();
            //foreach (var i in gameState.Hand.Cards) WriteLine(i.Name);
            //WriteLine();
            //WriteLine("OtherHand");
            //WriteLine();
            //foreach (var i in gameState.OtherHand.Cards) WriteLine(i.Name);
            //WriteLine();
            //WriteLine("Field");
            //WriteLine();
            //foreach (var i in gameState.Field.Cards) WriteLine(i.Name);
            //WriteLine();
            //WriteLine("OtherField");
            //WriteLine();
            //foreach (var i in gameState.OtherField.Cards) WriteLine(i.Name);
            //WriteLine();
            //WriteLine("Graveyard");
            //WriteLine();
            //foreach (var i in gameState.Graveyard.Cards) WriteLine(i.Name);
            //WriteLine();
            //WriteLine("OtherGraveyard");
            //WriteLine();
            //foreach (var i in gameState.OtherGraveyard.Cards) WriteLine(i.Name);
            //WriteLine();
        }
    }
}