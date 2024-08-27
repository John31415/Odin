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

        public TokenDictionary() {
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

    public enum TokenType
    {
        #region Single-character tokens

        SEMICOLON, // ;
        COMMA, // ,
        COLON, // :
        DOT, // .
        LEFT_PAREN, // (
        RIGHT_PAREN, // )
        LEFT_BRACE, // [
        RIGHT_BRACE, // ]
        LEFT_CURLY, // {
        RIGHT_CURLY, // }
        PLUS, // +
        MINUS, // -
        STAR, // *
        SLASH, // /
        QUOT, // "
        EQUAL, // =
        LESS, // <
        GREATER, // >
        AT, // @
        NOT, // !
        AND, // &
        OR, // |
        XOR, // ^
        MOD, // %
        EXP, // ~

        #endregion

        #region Double-character tokens

        GREATER_EQUAL, // >=
        LESS_EQUAL, // <=
        EQUAL_EQUAL, // ==
        PLUS_PLUS, // ++
        MINUS_MINUS, // --
        AND_AND, // &&
        OR_OR, // ||
        AT_AT, // @@
        AT_EQUAL, // @=
        LAMBDA, // =>
        STAR_EQUAL, // *=
        PLUS_EQUAL, // +=
        MINUS_EQUAL, // -=
        SLASH_EQUAL, // /=
        NOT_EQUAL, // !=
        MOD_EQUAL, // %=
        AND_EQUAL, // &=
        OR_EQUAL, // |=
        XOR_EQUAL, // ^=
        EXP_EQUAL, // ~=
        LEFT_SHIFT, // <<
        RIGHT_SHIFT, // >>

        #endregion

        #region Triple-character tokens

        AT_AT_EQUAL, // @@=
        LEFT_SHIFT_EQUAL, // <<=
        RIGHT_SHIFT_EQUAL, // >>=

        #endregion

        #region Literals

        IDENTIFIER,
        STRING, // String
        NUMBER, // Number
        BOOL, // Bool

        #endregion

        #region Keywords

        CLASS_EFFECT, // effect
        CLASS_CARD, // card
        EFFECT, // Effect
        NAME, // Name
        PARAMS, // Params
        AMOUNT, // Amount
        ACTION, // Action
        //TRIGGER_PLAYER, // TriggerPlayer
        //BOARD, // Board
        //HAND_OF_PLAYER, // HandOfPlayer
        //HAND, // Hand
        //FIELD_OF_PLAYER, // FieldOfPlayer
        //FIELD, // Field
        //GRAVEYARD_OF_PLAYER, // GraveyardOfPlayer
        //GRAVEYARD, // Graveyard
        //DECK_OF_PLAYER, // DeckOfPlayer
        //DECK, // Deck
        //OWNER, // Owner
        //FIND, // Find
        //PUSH, // Push
        //SEND_BOTTOM, // SendBottom
        //POP, // Pop
        //REMOVE, // Remove
        //SHUFFLE, // Shuffle
        TYPE, // Type
        FACTION, // Faction
        POWER, // Power
        RANGE, // Range
        ON_ACTIVATION, // OnActivation
        SELECTOR, // Selector
        SOURCE, // Source
        SINGLE, // Single
        PREDICATE, // Predicate
        POST_ACTION, // PostAction

        TRUE, // true
        FALSE, // false
        FOR, // for
        IN, // in
        WHILE, // while

        //S_HAND, // hand
        //S_OTHER_HAND, // otherHand
        //S_DECK, // deck
        //S_OTHER_DECK, // otherDeck
        //S_FIELD, // field
        //S_OTHER_FIELD, // otherField
        //S_PARENT, // parent

        #endregion

        EOF
    }
}
