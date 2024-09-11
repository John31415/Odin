using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Token
    {
        internal TokenType _type;
        internal string _lexeme;
        internal object _literal;
        internal int _line;
        internal int _column;
        internal int _position;
        internal int _lineBeginning;

        public Token(TokenType type, string lexeme, object literal, int line, int column, int position, int lineBeginning)
        {
            _type = type;
            _lexeme = lexeme;
            _literal = literal;
            _line = line;
            _column = column;
            _position = position;
            _lineBeginning = lineBeginning;
        }

        public override string ToString()
        {
            return _type + " " + _lexeme + " " + _literal;
        }
    }
}
