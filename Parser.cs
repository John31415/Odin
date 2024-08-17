using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Odin
{
    internal class Parser<T>
    {
        private List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        internal List<Stmt<T>> Parse()
        {
            List<Stmt<T>> statements = new List<Stmt<T>>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }
            return statements;
        }

        private Stmt<T> Declaration()
        {
            List<TokenType> identifier = new List<TokenType> { TokenType.IDENTIFIER };
            if (Match(identifier))
            {
                return VarDeclaration();
            }
            return Statement();
        }

        private Stmt<T> VarDeclaration()
        {
            Token name = Previous();
            Consume(TokenType.EQUAL, "Expected '=' after variable name.");
            Expr<T> initializer = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after variable declaration.");
            return new Var<T>(name, initializer);
        }

        private Stmt<T> Statement()
        {
            return ExpressionStatement();
        }

        private Stmt<T> ExpressionStatement()
        {
            Expr<T> expr = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return new Expression<T>(expr);
        }

        private Expr<T> Expression() => Logic();

        private Expr<T> Logic()
        {
            Expr<T> expr = Equality();
            List<TokenType> operators = new List<TokenType> { TokenType.AND_AND, TokenType.OR_OR };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Equality();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Equality()
        {
            Expr<T> expr = Comparison();
            List<TokenType> operators = new List<TokenType> { TokenType.NOT_EQUAL, TokenType.EQUAL_EQUAL };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Comparison();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Comparison()
        {
            Expr<T> expr = Shifft();
            List<TokenType> operators = new List<TokenType> { TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Shifft();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Shifft()
        {
            Expr<T> expr = Bitwise();
            List<TokenType> operators = new List<TokenType> { TokenType.LEFT_SHIFT, TokenType.RIGHT_SHIFT };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Bitwise();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Bitwise()
        {
            Expr<T> expr = Term();
            List<TokenType> operators = new List<TokenType> { TokenType.OR, TokenType.AND, TokenType.XOR };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Term();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Term()
        {
            Expr<T> expr = Factor();
            List<TokenType> operators = new List<TokenType> { TokenType.PLUS, TokenType.MINUS, TokenType.AT, TokenType.AT_AT };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Factor();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Factor()
        {
            Expr<T> expr = Expo();
            List<TokenType> operators = new List<TokenType> { TokenType.STAR, TokenType.SLASH, TokenType.MOD };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Expo();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Expo()
        {
            Expr<T> expr = Unary();
            List<TokenType> operators = new List<TokenType> { TokenType.EXP };
            while (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Unary();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> Unary()
        {
            List<TokenType> operators = new List<TokenType> { TokenType.NOT, TokenType.MINUS };
            if (Match(operators))
            {
                Token oper = Previous();
                Expr<T> right = Unary();
                return new Unary<T>(oper, right);
            }
            return Primary();
        }

        private Expr<T> Primary()
        {
            List<TokenType> operators = new List<TokenType> { TokenType.FALSE };
            if (Match(operators))
            {
                return new Literal<T>(false);
            }
            operators = new List<TokenType> { TokenType.TRUE };
            if (Match(operators))
            {
                return new Literal<T>(true);
            }
            operators = new List<TokenType> { TokenType.NUMBER, TokenType.STRING };
            if (Match(operators))
            {
                return new Literal<T>(Previous()._literal);
            }
            operators = new List<TokenType> { TokenType.IDENTIFIER };
            if (Match(operators))
            {
                return new Variable<T>(Previous());
            }
            operators = new List<TokenType> { TokenType.LEFT_PAREN };
            if (Match(operators))
            {
                Expr<T> expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new Grouping<T>(expr);
            }
            ThrowError(Peek(), "Expected expression.");
            return null!;
        }

        private bool Match(List<TokenType> operators)
        {
            foreach (TokenType oper in operators)
            {
                if (Check(oper))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool Check(TokenType oper)
        {
            if (IsAtEnd()) return false;
            return Peek()._type == oper;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd() => Peek()._type == TokenType.EOF;

        private Token Peek() => _tokens[_current];

        private Token Previous() => _tokens[_current - 1];

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            ThrowError(Previous(), message);
            Synchronize();
            return null!;
        }

        private void ThrowError(Token token, string message)
        {
            ErrorReporter.ThrowError(message, token._line, token._position, token._lineBeginning);
        }

        private void Synchronize()
        {
            Advance();
            TokenDictionary tokenDictionary = new TokenDictionary();
            Dictionary<string, TokenType> StatementBeginning = tokenDictionary.StatementBeginning;
            while (!IsAtEnd())
            {
                if (Previous()._type == TokenType.SEMICOLON || StatementBeginning.ContainsValue(Peek()._type)) return;
                Advance();
            }
        }
    }
}
