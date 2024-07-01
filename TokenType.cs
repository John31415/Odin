using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
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
        STRING,
        NUMBER,
        BOOL,

        #endregion

        #region Keywords

        CLASS_EFFECT, // effect
        CLASS_CARD, // card
        NAME, // Name
        PARAMS, // Params
        ACTION, // Action
        TARGETS, // targets
        TARGET, // target
        CONTEXT, // context
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
        UNIT, // unit
        POST_ACTION, // PostAction

        #region Syntaxis

        TRUE, // true
        FALSE, // false
        FOR, // for
        IN, // in
        WHILE, // while

        #endregion

        #region Range

        MELEE, // Melee
        RANGED, // Ranged
        SIEGE, // Siege

        #endregion

        #region Source

        S_HAND, // hand
        S_OTHER_HAND, // otherHand
        S_DECK, // deck
        S_OTHER_DECK, // otherDeck
        S_FIELD, // field
        S_OTHER_FIELD, // otherField
        S_PARENT, // parent

        #endregion

        #endregion

        EOF
    }
}
