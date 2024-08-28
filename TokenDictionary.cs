using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    public class TokenDictionary
    {
        public Dictionary<string, TokenType> Keywords;
        public Dictionary<string, TokenType> StatementBeginning;

        public TokenDictionary()
        {
            Keywords = new Dictionary<string, TokenType>();
            StatementBeginning = new Dictionary<string, TokenType>();

            Keywords["Action"] = StatementBeginning["Action"] = TokenType.ACTION;//
            Keywords["Bool"] = StatementBeginning["Bool"] = TokenType.BOOL;//
            Keywords["Effect"] = StatementBeginning["Effect"] = TokenType.EFFECT;//
            Keywords["OnActivation"] = StatementBeginning["OnActivation"] = TokenType.ON_ACTIVATION;//
            Keywords["Params"] = StatementBeginning["Params"] = TokenType.PARAMS;//
            Keywords["PostAction"] = StatementBeginning["PostAction"] = TokenType.POST_ACTION;//
            Keywords["Predicate"] = StatementBeginning["Predicate"] = TokenType.PREDICATE;//
            Keywords["Selector"] = StatementBeginning["Selector"] = TokenType.SELECTOR;//
            Keywords["Single"] = StatementBeginning["Single"] = TokenType.SINGLE;//
            Keywords["Source"] = StatementBeginning["Source"] = TokenType.SOURCE;//
            Keywords["card"] = StatementBeginning["card"] = TokenType.CLASS_CARD;//
            Keywords["effect"] = StatementBeginning["effect"] = TokenType.CLASS_EFFECT;//
            Keywords["for"] = StatementBeginning["for"] = TokenType.FOR;//
            Keywords["while"] = StatementBeginning["while"] = TokenType.WHILE;//
            StatementBeginning["Faction"] = TokenType.FACTION;//
            StatementBeginning["Name"] = TokenType.NAME;//
            StatementBeginning["Power"] = TokenType.POWER;//
            StatementBeginning["Range"] = TokenType.RANGE;//
            StatementBeginning["Type"] = TokenType.TYPE;//

            //Keywords["Board"] = TokenType.BOARD;
            //Keywords["Deck"] = TokenType.DECK;
            //Keywords["DeckOfPlayer"] = TokenType.DECK_OF_PLAYER;
            //Keywords["Field"] = TokenType.FIELD;
            //Keywords["FieldOfPlayer"] = TokenType.FIELD_OF_PLAYER;
            //Keywords["Find"] = TokenType.FIND;
            //Keywords["Graveyard"] = TokenType.GRAVEYARD;
            //Keywords["GraveyardOfPlayer"] = TokenType.GRAVEYARD_OF_PLAYER;
            //Keywords["Hand"] = TokenType.HAND;
            //Keywords["HandOfPlayer"] = TokenType.HAND_OF_PLAYER;
            Keywords["Number"] = TokenType.NUMBER;
            //Keywords["Owner"] = TokenType.OWNER;
            //Keywords["Pop"] = TokenType.POP;
            //Keywords["Push"] = TokenType.PUSH;
            //Keywords["Remove"] = TokenType.REMOVE;
            //Keywords["SendBottom"] = TokenType.SEND_BOTTOM;
            //Keywords["Shuffle"] = TokenType.SHUFFLE;
            Keywords["String"] = TokenType.STRING;
            //Keywords["TriggerPlayer"] = TokenType.TRIGGER_PLAYER;
            //Keywords["deck"] = TokenType.S_DECK;
            Keywords["false"] = TokenType.FALSE;
            //Keywords["field"] = TokenType.S_FIELD;
            //Keywords["hand"] = TokenType.S_HAND;
            Keywords["in"] = TokenType.IN;
            //Keywords["otherDeck"] = TokenType.S_OTHER_DECK;
            //Keywords["otherField"] = TokenType.S_OTHER_FIELD;
            //Keywords["otherHand"] = TokenType.S_OTHER_HAND;
            //Keywords["parent"] = TokenType.S_PARENT;
            Keywords["true"] = TokenType.TRUE;

        }
    }
}
