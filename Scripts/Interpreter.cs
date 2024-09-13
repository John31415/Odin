using System;
using System.Collections.Generic;
using static System.Console;

namespace Odin
{
    internal class Interpreter : IVisitor<object>
    {
        internal Environment globals = new Environment();
        private Environment environment = new Environment();
        private GameState gameState;
        private Lists lastList;
        private Card lastCard;
        private object parentTargets;
        private Lists actualSource;
        private Dictionary<string, (Dictionary<string, TokenType>, Method<object>)> effectFun;
        private Lists TARGETS;

        internal Interpreter()
        {
            environment = globals;
            globals.Define("rand", new Rand());
            globals.Define("log", new Log());
            globals.Define("max", new Max());
            globals.Define("min", new Min());
            parentTargets = null;
        }

        internal Interpreter(GameState _gameState)
        {
            gameState = _gameState;
            environment = globals;
            globals.Define("rand", new Rand());
            globals.Define("log", new Log());
            globals.Define("max", new Max());
            globals.Define("min", new Min());
            parentTargets = null;
        }

        internal Dictionary<Card, Method<object>> CreateCards(List<Class<object>> classes)
        {
            Dictionary<Card, Method<object>> cards = new Dictionary<Card, Method<object>>();
            foreach (var klass in classes)
            {
                if (klass is EffectClass<object>)
                {
                    Run.effects.Add((EffectClass<object>)klass);
                    continue;
                }
                Card card = (Card)Execute(klass);
                if (card != null)
                {
                    cards[card] = ((CardClass<object>)klass)._onActivation;
                }
            }
            return cards;
        }

        internal void AplyEffect(List<EffectClass<object>> effects, Method<object> OnActivation)
        {
            effectFun = new Dictionary<string, (Dictionary<string, TokenType>, Method<object>)>();
            foreach (var effect in effects)
            {
                Execute(effect);
            }
            Execute(OnActivation);
        }

        private object Execute(Class<object> klass) => klass.Accept(this);

        private object Execute(Method<object> method) => method.Accept(this);

        private object Execute(Prop<object> prop) => prop.Accept(this);

        private object Execute(Stmt<object> stmt) => stmt.Accept(this);

        public object VisitCardClassClass(CardClass<object> cardClass)
        {
            object type = Execute(cardClass._type);
            object name = Execute(cardClass._name);
            object faction = Execute(cardClass._faction);
            object power = Execute(cardClass._power);
            object range = Execute(cardClass._range);
            return new Card(0, 0, (string)type, (string)name, (string)faction, (long)power, (string)range, "");
        }

        public object VisitEffectClassClass(EffectClass<object> effectClass)
        {
            object name = Execute(effectClass._name);
            object _params = new Dictionary<string, TokenType>();
            if (effectClass._params != null)
            {
                _params = Execute(effectClass._params);
            }
            effectFun[(string)name] = ((Dictionary<string, TokenType>, Method<object>))(_params, effectClass._action);
            return null;
        }

        public object VisitOnActivationMethod(OnActivation<object> onActivation)
        {
            foreach(var onAct in onActivation._onActBodies)
            {
                Execute(onAct);
            }
            return null;
        }

        public object VisitOnActBodyMethod(OnActBody<object> onActBody)
        {
            object effect = Execute(onActBody._effect);
            Token effectToken;
            string nameEffect;
            Dictionary<string, object> paramsEffect = new Dictionary<string, object>();
            if(effect is ValueTuple<Token, string>)
            {
                (effectToken, nameEffect) = ((Token, string))effect;
            }
            else
            {
                (effectToken, nameEffect, paramsEffect) = ((Token, string, Dictionary<string, object>))effect;
            }
            object selector, postAction;
            if(onActBody._selector != null)
            {
                selector = Execute(onActBody._selector);
            }
            else
            {
                selector = parentTargets;
            }
            DoActionEffect(effectToken, nameEffect, paramsEffect, (Lists)selector);
            parentTargets = selector;
            if(onActBody._postAction != null)
            {
                postAction = Execute(onActBody._postAction);
            }
            return null;
        }

        public object VisitEffectMethod(Effect<object> effect)
        {
            Dictionary<string, object> _params = new Dictionary<string, object>();
            if(effect._exprName != null)
            {
                return (effect._effect, (string)Evaluate(effect._exprName));
            }
            object name = Execute(effect._name);
            foreach(var _param in effect._paramsv)
            {
                string paramName;
                object paramValue;
                (paramName, paramValue) = ((string, object))Execute(_param);
                _params[paramName] = paramValue;
            }
            return (effect._effect, (string)name,  _params);
        }

        public object VisitSelectorMethod(Selector<object> selector)
        {
            object source = Execute(selector._source);
            if (source == null) return null;
            object single = Execute(selector._single);
            if (single == null) return null;
            actualSource = (Lists)source;
            object predicate = Execute(selector._predicate);
            actualSource = null;
            if (predicate == null) return null;
            if (((Lists)predicate).Cards.Count == 0)
            {
                return (Lists)predicate;
            }
            List<Card> list = ((Lists)predicate).Cards;
            if ((bool)single)
            {
                list = new List<Card> { ((Lists)predicate).Cards[0] };
            }
            return new Lists(list);
        }

        public object VisitParamsMethod(Params<object> _params)
        {
            Dictionary<string, TokenType> paramsDict = new Dictionary<string, TokenType>();
            foreach (var _param in _params._paramsList)
            {
                string name;
                TokenType value;
                (name, value) = ((string, TokenType))Execute(_param);
                paramsDict[name] = value;
            }
            return paramsDict;
        }

        public object VisitActionMethod(Action<object> action)
        {
            environment.Define(action._targets._lexeme, TARGETS);
            environment.Define(action._context._lexeme, new Context(gameState));
            foreach(var stmt in action._stmts)
            {
                Execute(stmt);
            }
            return null;
        }

        public object VisitTypeProp(Type<object> type)
        {
            object value = Evaluate(type._value);
            if (!(value is string))
            {
                ThrowError(type._type, "'Type' must be a string.");
                return null;
            }
            return value;
        }

        public object VisitNameProp(Name<object> name)
        {
            object value = Evaluate(name._value);
            if (!(value is string))
            {
                ThrowError(name._name, "'Name' must be a string.");
                return null;
            }
            return value;
        }

        public object VisitFactionProp(Faction<object> faction)
        {
            object value = Evaluate(faction._value);
            if (!(value is string))
            {
                ThrowError(faction._faction, "'Faction' must be a string.");
                return null;
            }
            return value;
        }

        public object VisitPowerProp(Power<object> power)
        {
            object value = Evaluate(power._value);
            if (!(value is long))
            {
                ThrowError(power._power, "'Power' must be an integer.");
                return null;
            }
            return value;
        }

        public object VisitRangeProp(Range<object> range)
        {
            string rangeStr = "";
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs["Melee"] = "M";
            keyValuePairs["Ranged"] = "R";
            keyValuePairs["Siege"] = "S";
            foreach (var r in range._list)
            {
                object value = Evaluate(r);
                if (!(value is string))
                {
                    ThrowError(range._range, "'Range' must be a set of strings.");
                    return null;
                }
                if (!keyValuePairs.ContainsKey((string)value))
                {
                    ThrowError(range._range, "'Range' set can only have strings 'Melee', 'Ranged' or 'Siege'.");
                    return null;
                }
                rangeStr += keyValuePairs[(string)value];
            }
            return rangeStr;
        }

        public object VisitSourceProp(Source<object> source)
        {
            object value = Evaluate(source._value);
            if (value == null)
            {
                ThrowError(source._source, "'Source' parameter can't be null.");
                return null;
            }
            if (!(value is string))
            {
                ThrowError(source._source, "'Source' parameter must contain a 'string'.");
                return null;
            }
            if ((string)value == "board") return gameState.Board;
            if ((string)value == "hand") return gameState.Hand;
            if ((string)value == "otherHand") return gameState.OtherHand;
            if ((string)value == "field") return gameState.Field;
            if ((string)value == "otherField") return gameState.OtherField;
            if ((string)value == "deck") return gameState.Deck;
            if ((string)value == "otherDeck") return gameState.OtherDeck;
            if ((string)value == "parent")
            {
                if (parentTargets == null)
                {
                    ThrowError(source._source, "Unknown 'parent' targets");
                    return null;
                }
                return parentTargets;
            }
            ThrowError(source._source, "The value must be 'board', 'hand', 'otherHand', 'field', 'otherField', 'deck', 'otherDeck' or 'parent'");
            return null;
        }

        public object VisitSingleProp(Single<object> single)
        {
            object value = Evaluate(single._value);
            if (value == null)
            {
                ThrowError(single._single, "'Single' parameter can't be null.");
                return null;
            }
            return IsTrue(value);
        }

        public object VisitPredicateProp(Predicate<object> predicate)
        {
            List<Card> cards = new List<Card>();
            foreach (var card in actualSource.Cards)
            {
                Environment previous = environment;
                environment.Define(predicate._card._lexeme, card);
                bool ok = IsTrue(Evaluate(predicate._condition));
                if (ok)
                {
                    cards.Add(card);
                }
                environment = previous;
            }
            return new Lists(cards);
        }

        public object VisitParamDeclProp(ParamDecl<object> paramDecl) => (paramDecl._name._lexeme, paramDecl._value._type);

        public object VisitParamValueProp(ParamValue<object> paramValue)
        {
            return (paramValue._name._lexeme, Evaluate(paramValue._value));
        }

        public object VisitLiteralExpr(Literal<object> expr) => expr._value;

        public object VisitGroupingExpr(Grouping<object> expr) => Evaluate(expr._expression);

        public object VisitUnaryExpr(Unary<object> expr)
        {
            object right = Evaluate(expr._right);
            if (right == null)
            {
                ThrowError(expr._oper, "Operand can't be null.");
                return null;
            }
            switch (expr._oper._type)
            {
                case TokenType.NOT: return !IsTrue(right);
                case TokenType.MINUS: return -(long)right;
            }
            return null;
        }

        public object VisitBinaryExpr(Binary<object> expr)
        {
            object left = Evaluate(expr._left);
            object right = Evaluate(expr._right);
            if (left == null || right == null)
            {
                ThrowError(expr._oper, "Operands can't be null.");
                return null;
            }
            bool band;
            long l, r;
            switch (expr._oper._type)
            {
                case TokenType.AND_AND:
                    return (IsTrue(left) && IsTrue(right));
                case TokenType.OR_OR:
                    return (IsTrue(left) || IsTrue(right));
                case TokenType.EQUAL_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return IsEqual(left, right);
                    }
                    break;
                case TokenType.NOT_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return !IsEqual(left, right);
                    }
                    break;
                case TokenType.GREATER:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) > 0;
                    }
                    break;
                case TokenType.LESS:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) < 0;
                    }
                    break;
                case TokenType.GREATER_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) >= 0;
                    }
                    break;
                case TokenType.LESS_EQUAL:
                    if (CheckComparison(left, expr._oper, right))
                    {
                        return Compare(left, right) <= 0;
                    }
                    break;
                case TokenType.LEFT_SHIFT:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return (long)((int)l << (int)r);
                    }
                    break;
                case TokenType.RIGHT_SHIFT:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return (long)((int)l >> (int)r);
                    }
                    break;
                case TokenType.AND:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l & r;
                    }
                    break;
                case TokenType.OR:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l | r;
                    }
                    break;
                case TokenType.XOR:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l ^ r;
                    }
                    break;
                case TokenType.PLUS:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l + r;
                    }
                    break;
                case TokenType.MINUS:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l - r;
                    }
                    break;
                case TokenType.AT: return left.ToString() + right.ToString();
                case TokenType.AT_AT: return left.ToString() + " " + right.ToString();
                case TokenType.STAR:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band)
                    {
                        return l * r;
                    }
                    break;
                case TokenType.SLASH:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band && r != 0)
                    {
                        return l / r;
                    }
                    if (r == 0)
                    {
                        ThrowError(expr._oper, "Quotient must be a non-zero integer.");
                    }
                    break;
                case TokenType.MOD:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band && r != 0)
                    {
                        return l % r;
                    }
                    if (r == 0)
                    {
                        ThrowError(expr._oper, "Quotient must be a non-zero integer.");
                    }
                    break;
                case TokenType.EXP:
                    (band, l, r) = CheckNumbers(left, expr._oper, right);
                    if (band && r >= 0)
                    {
                        return QuickPow(l, r);
                    }
                    if (r < 0)
                    {
                        ThrowError(expr._oper, "Exponent must be a non-negative integer.");
                    }
                    break;
            }
            return null;
        }

        public object VisitIndexExpr(Index<object> expr)
        {
            object list = Evaluate(expr._list);
            if(!(list is Lists))
            {
                ThrowError(expr._brace, "Expected list before '['.");
                return null;
            }
            object index = Evaluate(expr._expr);
            if (!(index is long))
            {
                ThrowError(expr._brace, "Expected integer expression after '['");
                return null;
            }
            if (((Lists)list).Cards.Count <= (long)index)
            {
                ThrowError(expr._brace, $"Cannot access position '{(long)index}' in a list of '{((Lists)list).Cards.Count}' elements.");
                return null;
            }
            return ((Lists)list).Cards[Convert.ToInt32((long)index)];
        }

        public object VisitFindExpr(Find<object> expr)
        {
            object list = Evaluate(expr._expr);
            if (!(list is Lists))
            {
                ThrowError(expr._find, "Expected list before '('.");
                return null;
            }
            List<Card> cards = new List<Card>();
            foreach (var card in ((Lists)list).Cards)
            {
                Environment previous = environment;
                environment.Define(expr._card._lexeme, card);
                bool ok = IsTrue(Evaluate(expr._pred));
                if (ok)
                {
                    cards.Add(card);
                }
                environment = previous;
            }
            return new Lists(cards);
        }

        public object VisitCallExpr(Call<object> expr)
        {
            object callee = Evaluate(expr._callee);
            Lists auxList = lastList;
            List<object> arguments = new List<object>();
            foreach (var arg in expr._arguments)
            {
                arguments.Add(Evaluate(arg));
            }
            if (!(callee is ICallable))
            {
                ThrowError(expr._paren, "Can only call functions.");
                return null;
            }
            ICallable function = (ICallable)callee;
            if (arguments.Count != function.Arity())
            {
                ThrowError(expr._paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
                return null;
            }
            arguments.Add(auxList);
            return function.Call(gameState, this, arguments, expr._paren);
        }

        public object VisitGetExpr(Get<object> expr)
        {
            object obj = Evaluate(expr._obj);
            if (!(obj is IClass))
            {
                ThrowError(expr._name, "This must be an IClass.");
                return null;
            }
            if (((IClass)obj).properties.ContainsKey(expr._name._lexeme))
            {
                if (obj is Lists) lastList = (Lists)obj;
                else lastList = null;
                if (obj is Card) lastCard = (Card)obj;
                else lastCard = null;
                return ((IClass)obj).properties[expr._name._lexeme];
            }
            ThrowError(expr._name, "This is not a property or a method.");
            return null;
        }

        public object VisitSetExpr(Set<object> expr)
        {
            object obj = Evaluate(expr._obj);
            if (lastCard == null)
            {
                ThrowError(expr._name, "Arithmetic operations can only be aplaied to 'Card'.");
                return null;
            }
            Card auxCard = lastCard;
            if (!auxCard.properties.ContainsKey(expr._name._lexeme))
            {
                ThrowError(expr._name, "This is not a property or a method.");
                return null;
            }
            object value = null;
            TokenType type = OperConverter(expr._oper._type);
            Token token = expr._oper;
            token._type = type;
            if (type == TokenType.EQUAL) value = Evaluate(expr._value);
            else if (type != expr._oper._type) value = null;
            else value = Evaluate(new Binary<object>(new Literal<object>(obj), token, new Literal<object>(Evaluate(expr._value))));
            auxCard.Set(expr._name._lexeme, value);
            return null;
        }

        public object VisitExpressionStmt(Expression<object> stmt)
        {
            return Evaluate(stmt._expression);
        }

        public object VisitVarStmt(Var<object> stmt)
        {
            object value = null;
            if (stmt._initializer == null)
            {
                environment.Define(stmt._name._lexeme, value);
                return value;
            }
            TokenType type = OperConverter(stmt._type._type);
            Token token = stmt._type;
            token._type = type;
            if (type == TokenType.EQUAL) value = Evaluate(stmt._initializer);
            else if (type != stmt._type._type) value = null;
            else value = Evaluate(new Binary<object>(new Variable<object>(stmt._name), token, stmt._initializer));
            environment.Define(stmt._name._lexeme, value);
            return value;
        }

        public object VisitVariableExpr(Variable<object> expr) => environment.Get(expr._name);

        public object VisitPostOperExpr(PostOper<object> postOper)
        {
            object value = Evaluate(postOper._var);
            Card auxCard = lastCard;
            if (!(value is long))
            {
                ThrowError(postOper._type, $"The object must contain an integer value.");
                return null;
            }
            long Value = (long)value;
            if (postOper._type._type == TokenType.PLUS_PLUS) Value++;
            else Value--;
            if (postOper._var is Variable<object>) environment.Define(((Variable<object>)postOper._var)._name._lexeme, Value);
            else
            {
                if (auxCard == null)
                {
                    ThrowError(postOper._type, "The operation can only be aplied to integer variables and Card properties.");
                    return null;
                }
                auxCard.Set(((Get<object>)postOper._var)._name._lexeme, Value);
            }
            return (long)value;
        }

        public object VisitPreOperExpr(PreOper<object> preOper)
        {
            object value = Evaluate(preOper._var);
            Card auxCard = lastCard;
            if (!(value is long))
            {
                ThrowError(preOper._type, $"The object must contain an integer value.");
                return null;
            }
            long Value = (long)value;
            if (preOper._type._type == TokenType.PLUS_PLUS) Value++;
            else Value--;
            if (preOper._var is Variable<object>) environment.Define(((Variable<object>)preOper._var)._name._lexeme, Value);
            else
            {
                if (auxCard == null)
                {
                    ThrowError(preOper._type, "The operation can only be aplied to integer variables and Card properties.");
                    return null;
                }
                auxCard.Set(((Get<object>)preOper._var)._name._lexeme, Value);
            }
            return Value;
        }

        public object VisitBlockStmt(Block<object> stmt)
        {
            ExecuteBlock(stmt._statements, new Environment(environment));
            return null;
        }

        public object VisitWhileStmt(While<object> stmt)
        {
            while (IsTrue(Evaluate(stmt._condition)))
            {
                Execute(stmt._body);
            }
            return null;
        }

        public object VisitForStmt(For<object> stmt)
        {
            object list = Evaluate(stmt._list);
            if (!(list is Lists))
            {
                ThrowError(stmt._iter, "Expected list for this iterator.");
                return null;
            }
            foreach (var item in ((Lists)list).Cards)
            {
                Environment previous = environment;
                environment.Define(stmt._iter._lexeme, item);
                Execute(stmt._body);
                environment = previous;
            }
            return null;
        }

        internal void DoActionEffect(Token effect, string nameEffect, Dictionary<string, object> paramsEffect, Lists targets)
        {
            if (!effectFun.ContainsKey(nameEffect))
            {
                ThrowError(effect, "The 'Name' of this 'Effect' was not found.");
                return;
            }
            Dictionary<string, TokenType> realParams = effectFun[nameEffect].Item1;
            if(paramsEffect.Count != realParams.Count)
            {
                ThrowError(effect, $"Effect {nameEffect} expect {realParams.Count} parameter(s).");
                return;
            }
            Environment previous = environment;
            foreach (var item in paramsEffect)
            {
                if (!realParams.ContainsKey(item.Key))
                {
                    ThrowError(effect, $"In Effect block, '{nameEffect}' does not expect parameter '{item.Key}'.");
                    return;
                }
                if((item.Value is long) || (realParams[item.Key] == TokenType.NUMBER))
                {
                    environment.Define(item.Key, item.Value);
                    continue;
                }
                if ((item.Value is bool) || (realParams[item.Key] == TokenType.BOOL))
                {
                    environment.Define(item.Key, item.Value);
                    continue;
                }
                if ((item.Value is string) || (realParams[item.Key] == TokenType.STRING))
                {
                    environment.Define(item.Key, item.Value);
                    continue;
                }
                ThrowError(effect, $"In Effect block, the parameter {item.Key} is not of the correct type.");
                environment = previous;
                return;
            }
            Method<object> action = effectFun[nameEffect].Item2;
            TARGETS = targets;
            Execute(action);
            environment = previous;
        }

        internal void ExecuteBlock(List<Stmt<object>> statements, Environment environment)
        {
            Environment previous = this.environment;
            this.environment = environment;
            foreach (Stmt<object> stmt in statements)
            {
                Execute(stmt);
            }
            this.environment = previous;
        }

        private object Evaluate(Expr<object> expr) => expr.Accept(this);

        private bool IsTrue(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            if (obj is long) return (long)obj != 0;
            if (obj is string) return (string)obj != "";
            return true;
        }

        private long QuickPow(long a, long b)
        {
            if (b == 0) return 0;
            long aux = 1;
            while (b > 0)
            {
                if ((b % 2) == 1) aux *= a;
                a *= a;
                b >>= 1;
            }
            return aux;
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        private bool CheckComparison(object a, Token token, object b)
        {
            if (a.GetType() != b.GetType())
            {
                ThrowError(token, "Operands must be of the same type.");
                return false;
            }
            if (token._type != TokenType.EQUAL_EQUAL)
            {
                if (a is bool)
                {
                    ThrowError(token, "Operands must be integers.");
                    return false;
                }
            }
            return true;
        }

        private (bool, long, long) CheckNumbers(object a, Token token, object b)
        {
            if ((a is string) || (b is string))
            {
                ThrowError(token, "Operands can't be strings.");
                return (false, 0, 0);
            }
            long _a, _b;
            if (a is long) _a = (long)a;
            else if ((bool)a == true) _a = 1;
            else _a = 0;
            if (b is long) _b = (long)b;
            else if ((bool)b == true) _b = 1;
            else _b = 0;
            return (true, _a, _b);
        }

        private int Compare(object a, object b) => (a is long) ? ((long)a).CompareTo((long)b) : ((string)a).CompareTo((string)b);

        private TokenType OperConverter(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.AT_EQUAL:
                    return TokenType.AT;
                case TokenType.STAR_EQUAL:
                    return TokenType.STAR;
                case TokenType.PLUS_EQUAL:
                    return TokenType.PLUS;
                case TokenType.MINUS_EQUAL:
                    return TokenType.MINUS;
                case TokenType.SLASH_EQUAL:
                    return TokenType.SLASH;
                case TokenType.MOD_EQUAL:
                    return TokenType.MOD;
                case TokenType.AND_EQUAL:
                    return TokenType.AND;
                case TokenType.OR_EQUAL:
                    return TokenType.OR;
                case TokenType.XOR_EQUAL:
                    return TokenType.XOR;
                case TokenType.EXP_EQUAL:
                    return TokenType.EXP;
                case TokenType.LEFT_SHIFT_EQUAL:
                    return TokenType.LEFT_SHIFT;
                case TokenType.RIGHT_SHIFT_EQUAL:
                    return TokenType.RIGHT_SHIFT;
                case TokenType.AT_AT_EQUAL:
                    return TokenType.AT_AT;
                case TokenType.EQUAL:
                    return TokenType.EQUAL;
            }
            return tokenType;
        }

        private void ThrowError(Token token, string message)
        {
            ErrorReporter.ThrowError(message, token);
        }
    }
}
