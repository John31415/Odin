using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        private string auxForItemName;
        private Card auxForItemValue;

        internal Interpreter(GameState _gameState)
        {
            gameState = _gameState;
            environment = globals;
            globals.Define("rand", new Rand());
            globals.Define("log", new Log());
            globals.Define("context", new Context(gameState));
        }

        public void Interpret(List<Stmt<object>> statements)
        {
            foreach (Stmt<object> stmt in statements)
            {
                Execute(stmt);
            }
        }

        private object Execute(Stmt<object> stmt)
        {
            return stmt.Accept(this);
        }

        public object VisitLiteralExpr(Literal<object> expr) => expr._value;

        public object VisitGroupingExpr(Grouping<object> expr) => Evaluate(expr._expression);

        public object VisitUnaryExpr(Unary<object> expr)
        {
            object right = Evaluate(expr._right);
            if (right == null)
            {
                ThrowError(expr._oper, "Operand can't be null.");
                return null!;
            }
            switch (expr._oper._type)
            {
                case TokenType.NOT: return !IsTrue(right);
                case TokenType.MINUS: return -(long)right;
            }
            return null!;
        }

        public object VisitBinaryExpr(Binary<object> expr)
        {
            object left = Evaluate(expr._left);
            object right = Evaluate(expr._right);
            if (left == null || right == null)
            {
                ThrowError(expr._oper, "Operands can't be null.");
                return null!;
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
            return null!;
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
                return null!;
            }
            ICallable function = (ICallable)callee;
            if (arguments.Count != function.Arity())
            {
                ThrowError(expr._paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
                return null!;
            }
            arguments.Add(auxList);//provisional
            return function.Call(gameState, this, arguments, expr._paren);
        }

        public object VisitGetExpr(Get<object> expr)
        {
            object obj = Evaluate(expr._obj);
            if (!(obj is IClass))
            {
                ThrowError(expr._name, "This must be an IClass.");
                return null!;
            }
            if (((IClass)obj).properties.ContainsKey(expr._name._lexeme))
            {
                if (obj is Lists) lastList = (Lists)obj;
                else lastList = null!;
                if (obj is Card) lastCard = (Card)obj;
                else lastCard = null!;
                return ((IClass)obj).properties[expr._name._lexeme];
            }
            ThrowError(expr._name, "This is not a property or a method.");
            return null!;
        }

        public object VisitSetExpr(Set<object> expr)
        {
            object obj = Evaluate(expr._obj);
            if (lastCard == null!)
            {
                ThrowError(expr._name, "Arithmetic operations can only be aplaied to 'Card'.");
                return null!;
            }
            Card auxCard = lastCard;
            if (!auxCard.properties.ContainsKey(expr._name._lexeme))
            {
                ThrowError(expr._name, "This is not a property or a method.");
                return null!;
            }
            object value = null!;
            TokenType type = OperConverter(expr._oper._type);
            Token token = expr._oper;
            token._type = type;
            if (type == TokenType.EQUAL) value = Evaluate(expr._value);
            else if (type != expr._oper._type) value = null!;
            else value = Evaluate(new Binary<object>(new Literal<object>(obj), token, new Literal<object>(Evaluate(expr._value))));
            auxCard.Set(expr._name._lexeme, value);
            return null!;
        }

        public object VisitExpressionStmt(Expression<object> stmt)
        {
            return Evaluate(stmt._expression);
        }

        public object VisitVarStmt(Var<object> stmt)
        {
            object value = null!;
            if (stmt._initializer == null)
            {
                environment.Define(stmt._name._lexeme, value);
                return value;
            }
            TokenType type = OperConverter(stmt._type._type);
            Token token = stmt._type;
            token._type = type;
            if (type == TokenType.EQUAL) value = Evaluate(stmt._initializer);
            else if (type != stmt._type._type) value = null!;
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
                return null!;
            }
            long Value = (long)value;
            if (postOper._type._type == TokenType.PLUS_PLUS) Value++;
            else Value--;
            if (postOper._var is Variable<object>) environment.Define(((Variable<object>)postOper._var)._name._lexeme, Value);
            else
            {
                if (auxCard == null!)
                {
                    ThrowError(postOper._type, "The operation can only be aplied to integer variables and Card properties.");
                    return null!;
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
                return null!;
            }
            long Value = (long)value;
            if (preOper._type._type == TokenType.PLUS_PLUS) Value++;
            else Value--;
            if (preOper._var is Variable<object>) environment.Define(((Variable<object>)preOper._var)._name._lexeme, Value);
            else
            {
                if (auxCard == null!)
                {
                    ThrowError(preOper._type, "The operation can only be aplied to integer variables and Card properties.");
                    return null!;
                }
                auxCard.Set(((Get<object>)preOper._var)._name._lexeme, Value);
            }
            return Value;
        }

        public object VisitBlockStmt(Block<object> stmt)
        {
            ExecuteBlock(stmt._statements, new Environment(environment));
            return null!;
        }

        public object VisitWhileStmt(While<object> stmt)
        {
            while (IsTrue(Evaluate(stmt._condition)))
            {
                Execute(stmt._body);
            }
            return null!;
        }

        public object VisitForStmt(For<object> stmt)
        {
            object list = Evaluate(stmt._list);
            if (!(list is Lists))
            {
                ThrowError(stmt._iter, "Expected list for this iterator.");
                return null!;
            }
            auxForItemName = stmt._iter._lexeme;
            foreach (var item in ((Lists)list).Cards)
            {
                auxForItemValue = item;
                Execute(stmt._body);
                auxForItemValue = null!;
            }
            auxForItemName = null!;
            return null!;
        }

        internal void ExecuteBlock(List<Stmt<object>> statements, Environment environment)
        {
            Environment previous = this.environment;
            if (auxForItemName != null!) environment.Define(auxForItemName, auxForItemValue);
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
