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
        private string _lexeme;
        internal object _literal;
        private int _line;
        private int _column;

        public Token(TokenType type, string lexeme, object literal, int line, int column)
        {
            _type = type;
            _lexeme = lexeme;
            _literal = literal;
            _line = line;
            _column = column;
        }

        public override string ToString()
        {
            return _type + " " + _lexeme + " " + _literal;
        }
    }
}
