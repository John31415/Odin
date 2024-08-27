using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    public class GameState
    {
        internal long TriggerPlayer { get; set; }
        internal Lists Board { get; set; }
        internal Lists Deck { get; set; }
        internal Lists OtherDeck { get; set; }
        internal Lists Hand { get; set; }
        internal Lists OtherHand { get; set; }
        internal Lists Field { get; set; }
        internal Lists OtherField { get; set; }
        internal Lists Graveyard { get; set; }
        internal Lists OtherGraveyard { get; set; }

        public GameState(long triggerPlayer, Lists board, Lists deck, Lists otherDeck, Lists hand, Lists otherHand, Lists field, Lists otherField, Lists graveyard, Lists otherGraveyard)
        {
            TriggerPlayer = triggerPlayer;
            Board = board;
            Deck = deck;
            OtherDeck = otherDeck;
            Hand = hand;
            OtherHand = otherHand;
            Field = field;
            OtherField = otherField;
            Graveyard = graveyard;
            OtherGraveyard = otherGraveyard;
        }
    }
}
