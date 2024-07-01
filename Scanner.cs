using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public Scanner(string source)
        {
            _source = source;
            TokenDictionary _tokenDictionary = new TokenDictionary();
            Keywords = _tokenDictionary.Keywords;
        }
        private string ErrorLine()
        {
            int iter = current - 1;
            for (int i = 0; i < 100 && iter < _source.Length - 1 && _source[iter] != '\n'; i++) iter++;
            return _source.Substring(lineBeginning, iter - lineBeginning + 1);
        }

        private void ThrowError(string message)
        {
            ErrorReporter.Error(line, current - lineBeginning - 1, ErrorLine(), message);
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
            _tokens.Add(new Token(type, text, literal, line, current - lineBeginning + 1));
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
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break; // <= <
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break; // >= >
                case '*': AddToken(Match('=') ? TokenType.STAR_EQUAL : TokenType.STAR); break; // *= *
                case '^': AddToken(Match('=') ? TokenType.EXP_EQUAL : TokenType.EXP); break; // ^= ^
                case '@': AddToken(Match('@') ? TokenType.AT_AT : TokenType.AT); break; // @@ @
                case '&':
                    if (Match('&')) AddToken(TokenType.AND_AND);
                    else ThrowError("Unexpected character.");
                    break;
                case '|':
                    if (Match('|')) AddToken(TokenType.OR_OR);
                    else ThrowError("Unexpected character.");
                    break;
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
            _tokens.Add(new Token(TokenType.EOF, "", null!, line, 1));
            return _tokens;
        }
    }
}
