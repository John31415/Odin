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
            List<Stmt<T>> properties = new List<Stmt<T>>();
            while (!IsAtEnd())
            {
                properties.Add(Declaration());
            }
            return properties;
        }

        private Class<T> CardClass()
        {
            if (!Consume(TokenType.CLASS_CARD, "Expected 'card'.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            Prop<T> type = Type();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Prop<T> name = Name();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Prop<T> faction = Faction();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Prop<T> power = Power();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Prop<T> range = Range();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Method<T> onActivation = OnActivation();
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
            return new CardClass<T>(type, name, faction, power, range, onActivation);
        }

        private Class<T> EffectClass()
        {
            if (!Consume(TokenType.CLASS_EFFECT, "Expected 'effect'.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            Prop<T> name = Name();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Method<T> _params = null!;
            if (Check(TokenType.PARAMS))
            {
                _params = Params();
                if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            }
            Method<T> action = Action();
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
            return new EffectClass<T>(name, _params, action);
        }

        private Method<T> Params()
        {
            if (!Consume(TokenType.PARAMS, "Expected 'Params'.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            List<Prop<T>> _params = new List<Prop<T>>();
            _params.Add(ParamDecl());
            List<TokenType> aux = new List<TokenType> { TokenType.RIGHT_CURLY };
            while(!Match(aux) && !IsAtEnd())
            {
                if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
                _params.Add(ParamDecl());
            }
            return new Params<T>(_params);
        }

        private Prop<T> ParamDecl()
        {
            if (!Consume(TokenType.IDENTIFIER, "Expected 'Identifier'.")) return null!;
            Token name = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            List<TokenType> aux = new List<TokenType> { TokenType.STRING, TokenType.NUMBER, TokenType.BOOL };
            if (!Match(aux))
            {
                ThrowError(name, "The parameter type can only be 'Number', 'String' and 'Bool'.");
                return null!;
            }
            Token value = Previous();
            return new ParamDecl<T>(name, value);
        }

        private Method<T> Action()
        {
            if (!Consume(TokenType.ACTION, "Expected 'Action'.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            if (!Consume(TokenType.LEFT_PAREN, "Expected '('.")) return null!;
            if (!Consume(TokenType.TARGETS, "Expected 'targets'.")) return null!;
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            if (!Consume(TokenType.CONTEXT, "Expected 'context'.")) return null!;
            if (!Consume(TokenType.RIGHT_PAREN, "Expected ')'.")) return null!;
            if (!Consume(TokenType.LAMBDA, "Expected \"=>\".")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            List<Stmt<T>> stmts = new List<Stmt<T>>();
            List<TokenType> aux = new List<TokenType> { TokenType.RIGHT_CURLY };
            while (!Match(aux) && !IsAtEnd())
            {
                stmts.Add(Declaration());
            }
            return new Action<T>(stmts);
        }

        private Method<T> OnActivation()
        {
            if (!Consume(TokenType.ON_ACTIVATION, "Expected 'OnActivation'.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            if (!Consume(TokenType.LEFT_BRACE, "Expected '['.")) return null!;
            List<TokenType> aux = new List<TokenType> { TokenType.RIGHT_BRACE };
            if (Match(aux)) return null!;
            List<Method<T>> onActBodies = new List<Method<T>>();
            onActBodies.Add(OnActBody());
            while (!IsAtEnd())
            {
                if (Match(aux)) break;
                if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
                onActBodies.Add(OnActBody());
            }
            return new OnActivation<T>(onActBodies);
        }

        private Method<T> OnActBody()
        {
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            Method<T> effect = Effect();
            List<TokenType> aux = new List<TokenType> { TokenType.RIGHT_CURLY };
            if (Match(aux))
            {
                return new OnActBody<T>(effect, null!, null!);
            }
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            aux = new List<TokenType> { TokenType.POST_ACTION };
            Method<T> postAction = null!;
            if (Match(aux))
            {
                if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
                postAction = OnActBody();
                return new OnActBody<T>(effect, null!, postAction);
            }
            Method<T> selector = Selector();
            aux = new List<TokenType> { TokenType.RIGHT_CURLY };
            if (Match(aux))
            {
                return new OnActBody<T>(effect, selector, null!);
            }
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            if (!Consume(TokenType.POST_ACTION, "Expected 'PostAction'.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            postAction = OnActBody();
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
            return new OnActBody<T>(effect, selector, postAction);
        }

        private Method<T> Effect()
        {
            if (!Consume(TokenType.EFFECT, "Expected 'Effect'.")) return null!;
            Token effect = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            List<TokenType> curly = new List<TokenType> { TokenType.LEFT_CURLY };
            if (!Match(curly))
            {
                Expr<T> effectName = Expression();
                return new Effect<T>(effect, effectName);
            }
            Prop<T> name = Name();
            List<Prop<T>> paramsv = new List<Prop<T>>();
            curly = new List<TokenType> { TokenType.RIGHT_CURLY };
            while (!Match(curly) && !IsAtEnd())
            {
                paramsv.Add(ParamValue());
            }
            return new Effect<T>(effect, name, paramsv);
        }

        private Method<T> Selector()
        {
            if (!Consume(TokenType.SELECTOR, "Expected 'Selector'.")) return null!;
            Token selector = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            Prop<T> source = Source();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Prop<T> single = Single();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            Prop<T> predicate = Predicate();
            return new Selector<T>(selector, source, single, predicate);
        }

        private Prop<T> Range()
        {
            if (!Consume(TokenType.RANGE, "Expected 'Range'.")) return null!;
            Token range = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            if (!Consume(TokenType.LEFT_BRACE, "Expected '['.")) return null!;
            List<Expr<T>> list = new List<Expr<T>>();
            list.Add(Expression());
            List<TokenType> aux = new List<TokenType> { TokenType.RIGHT_BRACE };
            while (!Match(aux) && !IsAtEnd())
            {
                if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
                list.Add(Expression());
            }
            return new Range<T>(range, list);
        }

        private Prop<T> Source()
        {
            if (!Consume(TokenType.SOURCE, "Expected 'Source'.")) return null!;
            Token source = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            Expr<T> value = Expression();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            return new Source<T>(source, value);
        }

        private Prop<T> Single()
        {
            if (!Consume(TokenType.SINGLE, "Expected 'Single'.")) return null!;
            Token single = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            Expr<T> value = Expression();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            return new Single<T>(single, value);
        }

        private Prop<T> Predicate()
        {
            if (!Consume(TokenType.PREDICATE, "Expected 'Predicate'.")) return null!;
            Token predicate = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            if (!Consume(TokenType.LEFT_PAREN, "Expected '('.")) return null!;
            if (!Consume(TokenType.IDENTIFIER, "Expected identifier.")) return null!;
            Token card = Previous();
            if (!Consume(TokenType.RIGHT_PAREN, "Expected ')'.")) return null!;
            if (!Consume(TokenType.LAMBDA, "Expected \"=>\".")) return null!;
            Expr<T> condition = Expression();
            return new Predicate<T>(predicate, card, condition);
        }

        private Prop<T> Name()
        {
            if (!Consume(TokenType.NAME, "Expected 'Name'.")) return null!;
            Token name = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            Expr<T> value = Expression();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            return new Name<T>(name, value);
        }

        private Prop<T> Type()
        {
            if (!Consume(TokenType.TYPE, "Expected 'Type'.")) return null!;
            Token type = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            Expr<T> value = Expression();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            return new Type<T>(type, value);
        }

        private Prop<T> Faction()
        {
            if (!Consume(TokenType.FACTION, "Expected 'Faction'.")) return null!;
            Token faction = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            Expr<T> value = Expression();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            return new Faction<T>(faction, value);
        }

        private Prop<T> Power()
        {
            if (!Consume(TokenType.POWER, "Expected 'Power'.")) return null!;
            Token power = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            Expr<T> value = Expression();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            return new Power<T>(power, value);
        }

        private Prop<T> ParamValue()
        {
            if (!Consume(TokenType.IDENTIFIER, "Expected 'Identifier'.")) return null!;
            Token name = Previous();
            if (!Consume(TokenType.COLON, "Expected ':'.")) return null!;
            Expr<T> value = Expression();
            return new ParamValue<T>(name, value);
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
            List<TokenType> opers = new List<TokenType> { TokenType.EQUAL, TokenType.AT_EQUAL, TokenType.STAR_EQUAL, TokenType.PLUS_EQUAL,
                TokenType.MINUS_EQUAL, TokenType.SLASH_EQUAL, TokenType.MOD_EQUAL, TokenType.AND_EQUAL, TokenType.OR_EQUAL, TokenType.XOR_EQUAL,
                TokenType.EXP_EQUAL, TokenType.LEFT_SHIFT_EQUAL, TokenType.RIGHT_SHIFT_EQUAL, TokenType.AT_AT_EQUAL };
            Token oper = Peek();
            if (!Match(opers))
            {
                if (oper._type == TokenType.PLUS_PLUS || oper._type == TokenType.MINUS_MINUS)
                {
                    _current--;
                    return ExpressionStatement();
                }
                ThrowError(Peek(), "Only assignment, call, increment and decrement can be used as a statement.");
                return new Var<T>(name, oper, new Literal<T>(null!));
            }
            Expr<T> initializer = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after variable assignation.");
            return new Var<T>(name, oper, initializer);
        }

        private Stmt<T> Statement()
        {
            List<TokenType> curly = new List<TokenType> { TokenType.LEFT_CURLY };
            if (Match(curly)) return new Block<T>(Block());
            List<TokenType> While = new List<TokenType> { TokenType.WHILE };
            if (Match(While)) return WhileStatement();
            List<TokenType> For = new List<TokenType> { TokenType.FOR};
            if (Match(For)) return ForStatement();
            return ExpressionStatement();
        }

        private Stmt<T> ForStatement()
        {
            if (!Consume(TokenType.IDENTIFIER, "Expected 'identifier'.")) return null!;
            Token iter = Previous();
            List<TokenType> aux = new List<TokenType> { TokenType.IDENTIFIER, TokenType.TARGETS, TokenType.CONTEXT };
            if (!Match(aux))
            {
                ThrowError(Previous(), "Expected list.");
                return null!;
            }
            Token list = Previous();
            bool band = false;
            if (Check(TokenType.LEFT_CURLY)) band = true;
            Stmt<T> stmts = Statement();
            if (band)
            {
                Consume(TokenType.SEMICOLON, "Expected ';' after for statement.");
            }
            return new For<T>(iter, list, stmts);
        }

        private Stmt<T> WhileStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected '(' after 'while'.");
            Expr<T> condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after 'condition'.");
            Stmt<T> body = Statement();
            return new While<T>(condition, body);
        }

        private List<Stmt<T>> Block()
        {
            List<Stmt<T>> statements = new List<Stmt<T>>();
            while (!Check(TokenType.RIGHT_CURLY) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }
            Consume(TokenType.RIGHT_CURLY, "Expected '}' after block.");
            return statements;
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
                Token var = Previous();
                operators = new List<TokenType> { TokenType.PLUS_PLUS, TokenType.MINUS_MINUS };
                if (Match(operators))
                {
                    return PostOper(var);
                }
                return new Variable<T>(Previous());
            }
            operators = new List<TokenType> { TokenType.LEFT_PAREN };
            if (Match(operators))
            {
                Expr<T> expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new Grouping<T>(expr);
            }
            operators = new List<TokenType> { TokenType.PLUS_PLUS, TokenType.MINUS_MINUS };
            if (Match(operators))
            {
                return PreOper();
            }
            ThrowError(Peek(), "Expected expression.");
            return new Literal<T>(null!);
        }

        private Expr<T> PostOper(Token var)
        {
            Expr<T> Expre = new PostOper<T>(new Variable<T>(var), Previous());
            return Expre;
        }

        private Expr<T> PreOper()
        {
            Token oper = Previous();
            Consume(TokenType.IDENTIFIER, $"Expected IDENTIFIER after '{oper._lexeme}'.");
            Expr<T> Expre = new PreOper<T>(oper, new Variable<T>(Previous()));
            return Expre;
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

        private bool Consume(TokenType type, string message)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
            ThrowError(Peek(), message);
            Synchronize();
            return false;
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
                if (Previous()._type == TokenType.SEMICOLON || StatementBeginning.ContainsValue(Peek()._type) || Peek()._type == TokenType.IDENTIFIER) return;
                Advance();
            }
        }
    }
}
