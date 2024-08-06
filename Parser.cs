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

        public Parser(List <Token> tokens) 
        {
            _tokens = tokens;
        }

        private Expr<T> expression() => equality();

        private Expr<T> equality()
        {
            Expr<T> expr = comparison();
            List<TokenType> operators = new List<TokenType> { TokenType.NOT_EQUAL, TokenType.EQUAL_EQUAL };
            while (match(operators))
            {
                Token oper = previous();
                Expr<T> right = comparison();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> comparison()
        {
            Expr<T> expr = shifft();
            List<TokenType> operators = new List<TokenType> { TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL };
            while(match(operators))
            {
                Token oper = previous();
                Expr<T> right = shifft();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> shifft()
        {
            Expr<T> expr = bitwise();
            List<TokenType> operators = new List<TokenType> { TokenType.LEFT_SHIFT, TokenType.RIGHT_SHIFT };
            while (match(operators))
            {
                Token oper = previous();
                Expr<T> right = bitwise();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> bitwise()
        {
            Expr<T> expr = term();
            List<TokenType> operators = new List<TokenType> { TokenType.OR, TokenType.AND, TokenType.XOR };
            while (match(operators))
            {
                Token oper = previous();
                Expr<T> right = term();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> term()
        {
            Expr<T> expr = factor();
            List<TokenType> operators = new List<TokenType> { TokenType.PLUS, TokenType.MINUS };
            while (match(operators))
            {
                Token oper = previous();
                Expr<T> right = factor();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }

        private Expr<T> factor()
        {
            Expr<T> expr = unary();
            List<TokenType> operators = new List<TokenType> { TokenType.STAR, TokenType.SLASH };
            while (match(operators))
            {
                Token oper = previous();
                Expr<T> right = unary();
                expr = new Binary<T>(expr, oper, right);
            }
            return expr;
        }
        private Expr<T> unary()
        {
            List<TokenType> operators = new List<TokenType> { TokenType.NOT, TokenType.MINUS };
            if (match(operators))
            {
                Token oper = previous();
                Expr<T> right = unary();
                return new Unary<T>(oper, right);
            }
            return primary();
        }

        private Expr<T> primary()
        {
            List<TokenType> operators = new List<TokenType> { TokenType.FALSE };
            if (match(operators)) return new Literal<T>(false);
            operators = new List<TokenType> { TokenType.TRUE };
            if (match(operators)) return new Literal<T>(true);
            operators = new List<TokenType> { TokenType.NUMBER, TokenType.STRING };
            if (match(operators)) return new Literal<T>(previous()._literal);
            operators = new List<TokenType> { TokenType.LEFT_PAREN };
            if (match(operators))
            {
                Expr<T> expr = expression();
                consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new Grouping<T>(expr);
            }
        }

        private bool match(List<TokenType> operators)
        {
            foreach (TokenType oper in operators)
            {
                if (check(oper))
                {
                    advance();
                    return true;
                }
            }
            return false;
        }

        private bool check(TokenType oper)
        {
            if (isAtEnd()) return false;
            return peek()._type == oper;
        }

        private Token advance()
        {
            if (!isAtEnd()) _current++;
            return previous();
        }

        private bool isAtEnd() => peek()._type == TokenType.EOF;

        private Token peek() => _tokens[_current++];

        private Token previous() => _tokens[_current - 1];
    }
}
