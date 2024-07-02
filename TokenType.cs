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
        public TokenDictionary() {
            Keywords = new Dictionary<string, TokenType>();
            Keywords["Action"] = TokenType.ACTION;
            Keywords["Board"] = TokenType.BOARD;
            Keywords["Bool"] = TokenType.BOOL;
            Keywords["Deck"] = TokenType.DECK;
            Keywords["DeckOfPlayer"] = TokenType.DECK_OF_PLAYER;
            Keywords["Faction"] = TokenType.FACTION;
            Keywords["Field"] = TokenType.FIELD;
            Keywords["FieldOfPlayer"] = TokenType.FIELD_OF_PLAYER;
            Keywords["Find"] = TokenType.FIND;
            Keywords["Graveyard"] = TokenType.GRAVEYARD;
            Keywords["GraveyardOfPlayer"] = TokenType.GRAVEYARD_OF_PLAYER;
            Keywords["Hand"] = TokenType.HAND;
            Keywords["HandOfPlayer"] = TokenType.HAND_OF_PLAYER;
            Keywords["Name"] = TokenType.NAME;
            Keywords["Number"] = TokenType.NUMBER;
            Keywords["OnActivation"] = TokenType.ON_ACTIVATION;
            Keywords["Owner"] = TokenType.OWNER;
            Keywords["Params"] = TokenType.PARAMS;
            Keywords["Pop"] = TokenType.POP;
            Keywords["PostAction"] = TokenType.POST_ACTION;
            Keywords["Power"] = TokenType.POWER;
            Keywords["Predicate"] = TokenType.PREDICATE;
            Keywords["Push"] = TokenType.PUSH;
            Keywords["Range"] = TokenType.RANGE;
            Keywords["Remove"] = TokenType.REMOVE;
            Keywords["Selector"] = TokenType.SELECTOR;
            Keywords["SendBottom"] = TokenType.SEND_BOTTOM;
            Keywords["Shuffle"] = TokenType.SHUFFLE;
            Keywords["Single"] = TokenType.SINGLE;
            Keywords["Source"] = TokenType.SOURCE;
            Keywords["String"] = TokenType.STRING;
            Keywords["TriggerPlayer"] = TokenType.TRIGGER_PLAYER;
            Keywords["Type"] = TokenType.TYPE;
            Keywords["card"] = TokenType.CLASS_CARD;
            Keywords["deck"] = TokenType.S_DECK;
            Keywords["effect"] = TokenType.CLASS_EFFECT;
            Keywords["false"] = TokenType.FALSE;
            Keywords["field"] = TokenType.S_FIELD;
            Keywords["for"] = TokenType.FOR;
            Keywords["hand"] = TokenType.S_HAND;
            Keywords["in"] = TokenType.IN;
            Keywords["otherDeck"] = TokenType.S_OTHER_DECK;
            Keywords["otherField"] = TokenType.S_OTHER_FIELD;
            Keywords["otherHand"] = TokenType.S_OTHER_HAND;
            Keywords["parent"] = TokenType.S_PARENT;
            Keywords["true"] = TokenType.TRUE;
            Keywords["while"] = TokenType.WHILE;
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
        EXP, // ^
        QUOT, // "
        EQUAL, // =
        LESS, // <
        GREATER, // >
        AT, // @

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
        LAMBDA, // =>
        STAR_EQUAL, // *=
        PLUS_EQUAL, // +=
        MINUS_EQUAL, // -=
        SLASH_EQUAL, // /=
        EXP_EQUAL, // ^=

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
        NAME, // Name
        PARAMS, // Params
        AMOUNT, // Amount
        ACTION, // Action
        TRIGGER_PLAYER, // TriggerPlayer
        BOARD, // Board
        HAND_OF_PLAYER, // HandOfPlayer
        HAND, // Hand
        FIELD_OF_PLAYER, // FieldOfPlayer
        FIELD, // Field
        GRAVEYARD_OF_PLAYER, // GraveyardOfPlayer
        GRAVEYARD, // Graveyard
        DECK_OF_PLAYER, // DeckOfPlayer
        DECK, // Deck
        OWNER, // Owner
        FIND, // Find
        PUSH, // Push
        SEND_BOTTOM, // SendBottom
        POP, // Pop
        REMOVE, // Remove
        SHUFFLE, // Shuffle
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

        S_HAND, // hand
        S_OTHER_HAND, // otherHand
        S_DECK, // deck
        S_OTHER_DECK, // otherDeck
        S_FIELD, // field
        S_OTHER_FIELD, // otherField
        S_PARENT, // parent

        #endregion

        EOF
    }
}
