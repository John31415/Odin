using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    public class GameState
    {
        public long TriggerPlayer { get; set; }
        public Lists Board { get; set; }
        public Lists Deck { get; set; }
        public Lists OtherDeck { get; set; }
        public Lists Hand { get; set; }
        public Lists OtherHand { get; set; }
        public Lists Field { get; set; }
        public Lists OtherField { get; set; }
        public Lists Graveyard { get; set; }
        public Lists OtherGraveyard { get; set; }

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
