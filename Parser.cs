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

        internal List<Class<T>> Parse()
        {
            List<Class<T>> classes = new List<Class<T>>();
            while (!IsAtEnd())
            {
                Class<T> _class = Class();
                if (_class == null) break;
                classes.Add(_class);
            }
            return classes;
        }
        
        private Class<T> Class()
        {
            if (Check(TokenType.CLASS_CARD))
            {
                return CardClass();
            }
            if (Check(TokenType.CLASS_EFFECT))
            {
                return EffectClass();
            }
            ThrowError(Peek(), "Only 'card' and 'effect' are accepted as class names.");
            return null!;
        }

        private Class<T> CardClass()
        {
            if (!Consume(TokenType.CLASS_CARD, "Expected 'card' declaration.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{' after 'card'.")) return null!;
            Prop<T> type = Type();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Type' declaration.")) return null!;
            Prop<T> name = Name();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Name' declaration.")) return null!;
            Prop<T> faction = Faction();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Faction' declaration.")) return null!;
            Prop<T> power = Power();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Power' declaration.")) return null!;
            Prop<T> range = Range();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Range' declaration.")) return null!;
            Method<T> onActivation = OnActivation();
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}' at the end of 'card'.")) return null!;
            return new CardClass<T>(type, name, faction, power, range, onActivation);
        }

        private Class<T> EffectClass()
        {
            if (!Consume(TokenType.CLASS_EFFECT, "Expected 'effect' declaration.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{' after 'effect'.")) return null!;
            Prop<T> name = Name();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Name' declaration.")) return null!;
            Method<T> _params = null!;
            if (Check(TokenType.PARAMS))
            {
                _params = Params();
                if (!Consume(TokenType.COMMA, "Expected ',' after 'Params' declaration.")) return null!;
            }
            Method<T> action = Action();
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}' at the end of 'effect'.")) return null!;
            return new EffectClass<T>(name, _params, action);
        }

        private Method<T> Params()
        {
            if (!Consume(TokenType.PARAMS, "Expected 'Params' declaration.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':' after 'Params' declaration.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            List<Prop<T>> _params = new List<Prop<T>>();
            do
            {
                _params.Add(ParamDecl());
            } while (Match(TokenType.COMMA));
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}' after parameters.")) return null!;
            return new Params<T>(_params);
        }

        private Prop<T> ParamDecl()
        {
            if (!Consume(TokenType.IDENTIFIER, "Expected parameter name.")) return null!;
            Token name = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after parameter name.")) return null!;
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
            if (!Consume(TokenType.ACTION, "Expected 'Action' declaration.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':' after 'Action'.")) return null!;
            if (!Consume(TokenType.LEFT_PAREN, "Expected '('.")) return null!;
            if (!Consume(TokenType.IDENTIFIER, "Expected identifier containing 'targets'.")) return null!;
            Token targets = Previous();
            if (!Consume(TokenType.COMMA, "Expected ','.")) return null!;
            if (!Consume(TokenType.IDENTIFIER, "Expected identifier containing 'context'.")) return null!;
            Token context = Previous();
            if (!Consume(TokenType.RIGHT_PAREN, "Expected ')'.")) return null!;
            if (!Consume(TokenType.LAMBDA, "Expected \"=>\".")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            List<Stmt<T>> stmts = new List<Stmt<T>>();
            do
            {
                stmts.Add(Declaration());
            } while (!Match(TokenType.RIGHT_CURLY));
            return new Action<T>(targets, context, stmts);
        }

        private Method<T> OnActivation()
        {
            if (!Consume(TokenType.ON_ACTIVATION, "Expected 'OnActivation' declaration.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':' after 'OnActivation'.")) return null!;
            if (!Consume(TokenType.LEFT_BRACE, "Expected '['.")) return null!;
            if (Match(TokenType.RIGHT_BRACE)) return null!;
            List<Method<T>> onActBodies = new List<Method<T>>();
            do
            {
                onActBodies.Add(OnActBody());
            } while (Match(TokenType.COMMA));
            if (!Consume(TokenType.RIGHT_BRACE, "Expected ']'.")) return null!;
            return new OnActivation<T>(onActBodies);
        }

        private Method<T> OnActBody()
        {
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            Method<T> effect = Effect();
            if (!Match(TokenType.COMMA))
            {
                if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
                return new OnActBody<T>(effect, null!, null!);
            }
            Method<T> postAction = null!;
            if (Match(TokenType.POST_ACTION))
            {
                if (!Consume(TokenType.COLON, "Expected ':' after 'PostAction'.")) return null!;
                postAction = OnActBody();
                if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
                return new OnActBody<T>(effect, null!, postAction);
            }
            Method<T> selector = Selector();
            if (!Check(TokenType.COMMA))
            {
                if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
                return new OnActBody<T>(effect, selector, null!);
            }
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Selector'.")) return null!;
            if (!Consume(TokenType.POST_ACTION, "Expected 'PostAction'.")) return null!;
            if (!Consume(TokenType.COLON, "Expected ':' after 'PostAction'.")) return null!;
            postAction = OnActBody();
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
            return new OnActBody<T>(effect, selector, postAction);
        }

        private Method<T> Effect()
        {
            if (!Consume(TokenType.EFFECT, "Expected 'Effect' declaration.")) return null!;
            Token effect = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Effect'.")) return null!;
            if (!Match(TokenType.LEFT_CURLY))
            {
                Expr<T> effectName = Expression();
                return new Effect<T>(effect, effectName);
            }
            Prop<T> name = Name();
            List<Prop<T>> paramsv = new List<Prop<T>>();
            while (Match(TokenType.COMMA))
            {
                paramsv.Add(ParamValue());
            }
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
            return new Effect<T>(effect, name, paramsv);
        }

        private Method<T> Selector()
        {
            if (!Consume(TokenType.SELECTOR, "Expected 'Selector' declaration.")) return null!;
            Token selector = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Selector'.")) return null!;
            if (!Consume(TokenType.LEFT_CURLY, "Expected '{'.")) return null!;
            Prop<T> source = Source();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Source'.")) return null!;
            Prop<T> single = Single();
            if (!Consume(TokenType.COMMA, "Expected ',' after 'Single'.")) return null!;
            Prop<T> predicate = Predicate();
            if (!Consume(TokenType.RIGHT_CURLY, "Expected '}'.")) return null!;
            return new Selector<T>(selector, source, single, predicate);
        }

        private Prop<T> Range()
        {
            if (!Consume("Range", "Expected 'Range' declaration.")) return null!;
            Token range = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Range'.")) return null!;
            if (!Consume(TokenType.LEFT_BRACE, "Expected '['.")) return null!;
            List<Expr<T>> list = new List<Expr<T>>();
            do
            {
                list.Add(Expression());
            } while (Match(TokenType.COMMA));
            if (!Consume(TokenType.RIGHT_BRACE, "Expected ']'.")) return null!;
            return new Range<T>(range, list);
        }

        private Prop<T> Source()
        {
            if (!Consume(TokenType.SOURCE, "Expected 'Source' declaration.")) return null!;
            Token source = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Source'.")) return null!;
            Expr<T> value = Expression();
            return new Source<T>(source, value);
        }

        private Prop<T> Single()
        {
            if (!Consume(TokenType.SINGLE, "Expected 'Single' declaration.")) return null!;
            Token single = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Single'.")) return null!;
            Expr<T> value = Expression();
            return new Single<T>(single, value);
        }

        private Prop<T> Predicate()
        {
            if (!Consume(TokenType.PREDICATE, "Expected 'Predicate' declaration.")) return null!;
            Token predicate = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Predicate'.")) return null!;
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
            if (!Consume("Name", "Expected 'Name' declaration.")) return null!;
            Token name = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Name'.")) return null!;
            Expr<T> value = Expression();
            return new Name<T>(name, value);
        }

        private Prop<T> Type()
        {
            if (!Consume("Type", "Expected 'Type' declaration.")) return null!;
            Token type = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Type'.")) return null!;
            Expr<T> value = Expression();
            return new Type<T>(type, value);
        }

        private Prop<T> Faction()
        {
            if (!Consume("Faction", "Expected 'Faction' declaration.")) return null!;
            Token faction = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Faction'.")) return null!;
            Expr<T> value = Expression();
            return new Faction<T>(faction, value);
        }

        private Prop<T> Power()
        {
            if (!Consume("Power", "Expected 'Power' declaration.")) return null!;
            Token power = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after 'Power'.")) return null!;
            Expr<T> value = Expression();
            return new Power<T>(power, value);
        }

        private Prop<T> ParamValue()
        {
            if (!Consume(TokenType.IDENTIFIER, "Expected parameter declaration.")) return null!;
            Token name = Previous();
            if (!Consume(TokenType.COLON, "Expected ':' after parameter name.")) return null!;
            Expr<T> value = Expression();
            return new ParamValue<T>(name, value);
        }
        
        private Stmt<T> Declaration()
        {
            if (Match(TokenType.IDENTIFIER))
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
                if (Check(TokenType.PLUS_PLUS) || Check(TokenType.MINUS_MINUS))
                {
                    _current--;
                    return ExpressionStatement();
                }
                if (Check(TokenType.DOT))
                {
                    _current--;
                    return new Expression<T>(CallMethods());
                }
                ThrowError(Peek(), "Only assignment, call, increment and decrement can be used as a statement.");
                return new Var<T>(name, oper, new Literal<T>(null!));
            }
            Expr<T> initializer = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after variable assignation.");
            return new Var<T>(name, oper, initializer);
        }

        private Expr<T> CallMethods()
        {
            Expr<T> expr = Call();
            if (Match(TokenType.SEMICOLON))
            {
                return expr;
            }
            if(!(expr is Get<T>) && !(expr is Variable<T>))
            {
                ThrowError(Peek(), "Invalid operation.");
                return null!;
            }
            List<TokenType> opers = new List<TokenType> { TokenType.EQUAL, TokenType.AT_EQUAL, TokenType.STAR_EQUAL, TokenType.PLUS_EQUAL,
                TokenType.MINUS_EQUAL, TokenType.SLASH_EQUAL, TokenType.MOD_EQUAL, TokenType.AND_EQUAL, TokenType.OR_EQUAL, TokenType.XOR_EQUAL,
                TokenType.EXP_EQUAL, TokenType.LEFT_SHIFT_EQUAL, TokenType.RIGHT_SHIFT_EQUAL, TokenType.AT_AT_EQUAL };
            Token name = Previous();
            Token oper = Peek();
            if (!Match(opers))
            {
                ThrowError(Peek(), "Invalid operation for properties or methods.");
                return null!;
            }
            Expr<T> initializer = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after assignation.");
            return new Set<T>(expr, name, oper, initializer);
        }

        private Stmt<T> Statement()
        {
            if (Match(TokenType.LEFT_CURLY)) return new Block<T>(Block());
            if (Match(TokenType.WHILE)) return WhileStatement();
            if (Match(TokenType.FOR)) return ForStatement();
            return ExpressionStatement();
        }

        private Stmt<T> ForStatement()
        {
            if (!Consume(TokenType.IDENTIFIER, "Expected 'identifier'.")) return null!;
            Token iter = Previous();
            if (!Consume(TokenType.IN, "Expected 'in' in 'for' declaration.")) return null!;
            Expr<T> list = Call();
            Stmt<T> body = Declaration();
            return new For<T>(iter, list, body);
        }

        private Stmt<T> WhileStatement()
        {
            if (!Consume(TokenType.LEFT_PAREN, "Expected '(' after 'while'.")) return null!;
            Expr<T> condition = Expression();
            if (!Consume(TokenType.RIGHT_PAREN, "Expected ')' after 'condition'.")) return null!;
            Stmt<T> body = Declaration();
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
            Consume(TokenType.SEMICOLON, "Expected ';'.");
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
            return Call();
        }

        private Expr<T> Call()
        {
            Expr<T> expr = Primary();
            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                {
                    expr = FinishCall(expr);
                }
                else if (Match(TokenType.DOT))
                {
                    if (!Consume(TokenType.IDENTIFIER, "Expected property name after '.'.")) return null!;
                    Token name = Previous();
                    expr = new Get<T>(expr, name);
                }
                else
                {
                    break;
                }
            }
            List<TokenType> opers = new List<TokenType> { TokenType.PLUS_PLUS, TokenType.MINUS_MINUS };
            if (Match(opers))
            {
                if(expr is Call<T>)
                {
                    ThrowError(((Call<T>)expr)._paren, "This operation can't be aplaied to methods.");
                    return null!;
                }
                return new PostOper<T>(expr, Previous());
            }
            return expr;
        }

        private Expr<T> FinishCall(Expr<T> callee)
        {
            List<Expr<T>> arguments = new List<Expr<T>>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(Expression());
                } while (Match(TokenType.COMMA));
            }
            if (!Consume(TokenType.RIGHT_PAREN, "Expected ')' after arguments.")) return null!;
            Token paren = Previous();
            return new Call<T>(callee, paren, arguments);
        }

        private Expr<T> Primary()
        {
            if (Match(TokenType.FALSE))
            {
                return new Literal<T>(false);
            }
            if (Match(TokenType.TRUE))
            {
                return new Literal<T>(true);
            }
            List<TokenType> operators = new List<TokenType> { TokenType.NUMBER, TokenType.STRING };
            if (Match(operators))
            {
                return new Literal<T>(Previous()._literal);
            }
            if (Match(TokenType.IDENTIFIER))
            {
                Token var = Previous();
                operators = new List<TokenType> { TokenType.PLUS_PLUS, TokenType.MINUS_MINUS };
                if (Match(operators))
                {
                    return PostOper(var);
                }
                return new Variable<T>(Previous());
            }
            if (Match(TokenType.LEFT_PAREN))
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
            if (!Consume(TokenType.IDENTIFIER, $"Expected IDENTIFIER after '{oper._lexeme}'.")) return null!;
            Expr<T> Expre = new PreOper<T>(oper, new Variable<T>(Previous()));
            if (Check(TokenType.DOT))
            {
                _current--;
                Expr<T> expr = Call();
                if(!(expr is Get<T>))
                {
                    ThrowError(oper, "Operation can only be aplaied to integer variables and Card properties.");
                    return null!;
                }
                Expre = new PreOper<T>(oper, expr);
            }
            return Expre;
        }

        private bool Match(TokenType oper)
        {
            if (Check(oper))
            {
                Advance();
                return true;
            }
            return false;
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

        private bool Check(string oper)
        {
            if (IsAtEnd()) return false;
            return Peek()._lexeme == oper;
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

        private bool Consume(string type, string message)
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
            ErrorReporter.ThrowError(message, token);
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
