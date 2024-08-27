using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Odin
{
    internal class Scanner
    {
        private string _source;
        private List<Token> _tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;
        private int lineBeginning = 0;
        private Dictionary<string, TokenType> Keywords;

        public static string? Source { get; private set; }

        public Scanner(string source)
        {
            _source = source;
            Source = source;
            TokenDictionary _tokenDictionary = new TokenDictionary();
            Keywords = _tokenDictionary.Keywords;
        }

        private void ThrowError(string message)
        {
            ErrorReporter.ThrowError(message, line, current, lineBeginning);
        }

        private bool IsAtEnd()
        {
            return current >= _source.Length;
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[current] != expected) return false;
            current++;
            return true;
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null!);
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = _source.Substring(start, current - start);
            _tokens.Add(new Token(type, text, literal, line, current - lineBeginning + 1, current, lineBeginning));
        }

        private char Advance()
        {
            return _source[current++];
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[current];
        }

        private void String()
        {
            while (Peek() != '\"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    line++;
                    lineBeginning = current;
                }
                Advance();
            }
            if (IsAtEnd())
            {
                ThrowError("Unterminated string.");
                return;
            }
            Advance();
            AddToken(TokenType.STRING, _source.Substring(start + 1, current - start - 2));
        }
        
        private bool IsDigit(char c)
        {
            return '0' <= c && c <= '9';
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();
            string number = _source.Substring(start, current - start);
            if (number.Length > 10)
            {
                ThrowError("Integer Overflow.");
                return;
            }
            long value = long.Parse(number);
            if (value > int.MaxValue) ThrowError("Integer Overflow.");
            else AddToken(TokenType.NUMBER, value);
        }

        private bool IsAlpha(char c)
        {
            return ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') || c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();
            string text = _source.Substring(start, current - start);
            TokenType type = TokenType.IDENTIFIER;
            if (Keywords.ContainsKey(text)) type = Keywords[text];
            AddToken(type);
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case ' ': break;
                case '\r': break;
                case '\t': break;
                case '\n': line++; lineBeginning = current; break;
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '[': AddToken(TokenType.LEFT_BRACE); break;
                case ']': AddToken(TokenType.RIGHT_BRACE); break;
                case '{': AddToken(TokenType.LEFT_CURLY); break;
                case '}': AddToken(TokenType.RIGHT_CURLY); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case ':': AddToken(TokenType.COLON); break;
                case '-': AddToken(Match('-') ? TokenType.MINUS_MINUS : Match('=') ? TokenType.MINUS_EQUAL : TokenType.MINUS); break; // -- -= -
                case '+': AddToken(Match('+') ? TokenType.PLUS_PLUS : Match('=') ? TokenType.PLUS_EQUAL : TokenType.PLUS); break; // ++ += +
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : Match('>') ? TokenType.LAMBDA : TokenType.EQUAL); break; // == => =
                case '<': AddToken(Match('<') ? Match('=') ? TokenType.LEFT_SHIFT_EQUAL : TokenType.LEFT_SHIFT : Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break; // <<= << <= <
                case '>': AddToken(Match('>') ? Match('=') ? TokenType.RIGHT_SHIFT_EQUAL : TokenType.RIGHT_SHIFT : Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break; // >>= >> >= >
                case '*': AddToken(Match('=') ? TokenType.STAR_EQUAL : TokenType.STAR); break; // *= *
                case '~': AddToken(Match('=') ? TokenType.EXP_EQUAL : TokenType.EXP); break; // ~= ~
                case '!': AddToken(Match('=') ? TokenType.NOT_EQUAL : TokenType.NOT); break; // != !
                case '&': AddToken(Match('&') ? TokenType.AND_AND : Match('=') ? TokenType.AND_EQUAL : TokenType.AND); break; // && &= &
                case '|': AddToken(Match('|') ? TokenType.OR_OR : Match('=') ? TokenType.OR_EQUAL : TokenType.OR); break; // |= |
                case '^': AddToken(Match('=') ? TokenType.XOR_EQUAL : TokenType.XOR); break; // ^= ^
                case '%': AddToken(Match('=') ? TokenType.MOD_EQUAL : TokenType.MOD); break; // %= %
                case '@': AddToken(Match('@') ? Match('=') ? TokenType.AT_AT_EQUAL : TokenType.AT_AT : Match('=') ? TokenType.AT_EQUAL : TokenType.AT); break; // @@= @@ @= @
                case '/':
                    if (Match('/')) // Comments
                    {
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else // /=
                    {
                        AddToken(Match('=') ? TokenType.SLASH_EQUAL : TokenType.SLASH);
                    }
                    break;
                case '\"': String(); break;
                default:
                    if (IsDigit(c)) Number();
                    else if (IsAlpha(c)) Identifier();
                    else ThrowError("Unexpected character.");
                    break;
            }
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            _tokens.Add(new Token(TokenType.EOF, "", null!, line, 1, current, lineBeginning));
            return _tokens;
        }
    }
}
