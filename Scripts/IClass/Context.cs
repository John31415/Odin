using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Context : IClass
    {
        public Dictionary<string, object> properties { get; set; }

        internal Context(GameState gameState)
        {
            properties = new Dictionary<string, object>();
            properties["TriggerPlayer"] = gameState.TriggerPlayer;
            properties["Board"] = gameState.Board;
            properties["HandOfPlayer"] = new HandOfPlayer();
            properties["DeckOfPlayer"] = new DeckOfPlayer();
            properties["FieldOfPlayer"] = new FieldOfPlayer();
            properties["GraveyardOfPlayer"] = new GraveyardOfPlayer();
            properties["Hand"] = gameState.Hand;
            properties["OtherHand"] = gameState.OtherHand;
            properties["Field"] = gameState.Field;
            properties["OtherField"] = gameState.OtherField;
            properties["Graveyard"] = gameState.Graveyard;
            properties["OtherGraveyard"] = gameState.OtherGraveyard;
            properties["Deck"] = gameState.Deck;
            properties["OtherDeck"] = gameState.OtherDeck;
        }
    }
}
