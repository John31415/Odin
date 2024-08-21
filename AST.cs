using Odin;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Console;
using static System.Math;

namespace Odin
{
    internal abstract class Expr<T>
    {
        internal abstract T Accept(IVisitor<T> visitor);
    }

    internal class Binary<T> : Expr<T>
    {
        internal Expr<T> _left;
        internal Token _oper;
        internal Expr<T> _right;
        internal Binary(Expr<T> left, Token oper, Expr<T> right)
        {
            _left = left;
            _oper = oper;
            _right = right;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitBinaryExpr(this);
    }

    internal class Grouping<T> : Expr<T>
    {
        internal Expr<T> _expression;
        internal Grouping(Expr<T> expression)
        {
            _expression = expression;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this);
    }

    internal class Literal<T> : Expr<T>
    {
        internal object _value;
        internal Literal(object value)
        {
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this);
    }

    internal class Unary<T> : Expr<T>
    {
        internal Token _oper;
        internal Expr<T> _right;
        internal Unary(Token oper, Expr<T> right)
        {
            _oper = oper;
            _right = right;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitUnaryExpr(this);
    }

    internal class Variable<T> : Expr<T>
    {
        internal Token _name;
        internal Variable(Token name)
        {
            _name = name;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitVariableExpr(this);
    }

    internal class PostOper<T> : Expr<T>
    {
        internal Variable<T> _var;
        internal Token _type;
        internal PostOper(Variable<T> var, Token type)
        {
            _var = var;
            _type = type;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPostOperExpr(this);
    }

    internal class PreOper<T> : Expr<T>
    {
        internal Token _type;
        internal Variable<T> _var;
        internal PreOper(Token type, Variable<T> var)
        {
            _type = type;
            _var = var;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPreOperExpr(this);
    }

    internal abstract class Stmt<T>
    {
        internal abstract T Accept(IVisitor<T> visitor);
    }

    internal class Expression<T> : Stmt<T>
    {
        internal Expr<T> _expression;
        internal Expression(Expr<T> expression)
        {
            _expression = expression;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitExpressionStmt(this);
    }

    internal class Var<T> : Stmt<T>
    {
        internal Token _name;
        internal Token _type;
        internal Expr<T> _initializer;
        internal Var(Token name, Token type, Expr<T> initializer)
        {
            _name = name;
            _type = type;
            _initializer = initializer;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitVarStmt(this);
    }

    internal class Block<T> : Stmt<T>
    {
        internal List<Stmt<T>> _statements;
        internal Block(List<Stmt<T>> statements)
        {
            _statements = statements;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitBlockStmt(this);
    }

    internal class While<T> : Stmt<T>
    {
        internal Expr<T> _condition;
        internal Stmt<T> _body;
        internal While(Expr<T> condition, Stmt<T> body)
        {
            _condition = condition;
            _body = body;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitWhileStmt(this);
    }

    internal class For<T> : Stmt<T>
    {
        internal Token _iter;
        internal Token _list;
        internal Stmt<T> _body;

        internal For(Token iter, Token list, Stmt<T> body)
        {
            _iter = iter;
            _list = list;
            _body = body;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitForStmt(this);
    }

    internal abstract class Prop<T>
    {
        internal abstract T Accept(IVisitor<T> visitor);
    }

    internal class Type<T> : Prop<T>
    {
        internal Token _type;
        internal Expr<T> _value;

        internal Type(Token type, Expr<T> value)
        {
            _type = type;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitTypeProp(this);
    }

    internal class Name<T> : Prop<T>
    {
        internal Token _name;
        internal Expr<T> _value;

        internal Name(Token name, Expr<T> value)
        {
            _name = name;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitNameProp(this);
    }

    internal class Faction<T> : Prop<T>
    {
        internal Token _faction;
        internal Expr<T> _value;

        internal Faction(Token faction, Expr<T> value)
        {
            _faction = faction;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitFactionProp(this);
    }

    internal class Power<T> : Prop<T>
    {
        internal Token _power;
        internal Expr<T> _value;

        internal Power(Token power, Expr<T> value)
        {
            _power = power;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPowerProp(this);
    }

    internal class Source<T> : Prop<T>
    {
        internal Token _source;
        internal Expr<T> _value;

        internal Source(Token source, Expr<T> value)
        {
            _source = source;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitSourceProp(this);
    }

    internal class Single<T> : Prop<T>
    {
        internal Token _single;
        internal Expr<T> _value;

        internal Single(Token single, Expr<T> value)
        {
            _single = single;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitSingleProp(this);
    }

    internal class Predicate<T> : Prop<T>
    {
        internal Token _predicate;
        internal Token _card;
        internal Expr<T> _condition;

        internal Predicate(Token predicate, Token card, Expr<T> condition)
        {
            _predicate = predicate;
            _card = card;
            _condition = condition;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitPredicateProp(this);
    }

    internal class Range<T> : Prop<T>
    {
        internal Token _range;
        internal List<Expr<T>> _list;

        internal Range(Token range, List<Expr<T>> list)
        {
            _range = range;
            _list = list;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitRangeProp(this);
    }

    internal abstract class Method<T>
    {
        internal abstract T Accept(IVisitor<T> visitor);
    }

    internal class OnActivation<T> : Method<T>
    {
        internal List<Method<T>> _onActBodies;

        internal OnActivation(List<Method<T>> onActBodies)
        {
            _onActBodies = onActBodies;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitOnActivationMethod(this);
    }

    internal class OnActBody<T> : Method<T>
    {
        internal Method<T> _effect;
        internal Method<T> _selector;
        internal Method<T> _postAction;

        internal OnActBody(Method<T> effect, Method<T> selector, Method<T> postAction)
        {
            _effect = effect;
            _selector = selector;
            _postAction = postAction;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitOnActBodyMethod(this);
    }

    internal class Effect<T> : Method<T>
    {
        internal Token _effect;
        internal Prop<T> _name;
        internal Expr<T> _exprName;
        internal List<Prop<T>> _paramsv;

        internal Effect(Token effect, Expr<T> name)
        {
            _effect = effect;
            _exprName = name;
            _name = null!;
            _paramsv = null!;
        }

        internal Effect(Token effect, Prop<T> name, List<Prop<T>> paramsv)
        {
            _effect = effect;
            _name = name;
            _paramsv = paramsv;
            _exprName = null!;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitEffectMethod(this);
    }

    internal class ParamDecl<T> : Prop<T>
    {
        internal Token _name;
        internal Token _value;

        internal ParamDecl(Token name, Token value)
        {
            _name = name;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitParamDeclProp(this);
    }

    internal class ParamValue<T> : Prop<T>
    {
        internal Token _name;
        internal Expr<T> _value;

        internal ParamValue(Token name, Expr<T> value)
        {
            _name = name;
            _value = value;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitParamValueProp(this);
    }

    internal class Selector<T> : Method<T>
    {
        internal Token _selector;
        internal Prop<T> _source;
        internal Prop<T> _single;
        internal Prop<T> _predicate;

        internal Selector(Token selector, Prop<T> source, Prop<T> single, Prop<T> predicate)
        {
            _selector = selector;
            _source = source;
            _single = single;
            _predicate = predicate;
        }
        internal override T Accept(IVisitor<T> visitor) => visitor.VisitSelectorMethod(this);
    }

    internal class Params<T> : Method<T>
    {
        internal List<Prop<T>> _paramsList;

        internal Params(List<Prop<T>> paramsList)
        {
            _paramsList = paramsList;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitParamsMethod(this);
    }

    internal class Action<T> : Method<T>
    {
        Token _targets;
        Token _context;
        internal List<Stmt<T>> _stmts;

        internal Action(Token targets, Token context, List<Stmt<T>> stmts)
        {
            _targets = targets;
            _context = context;
            _stmts = stmts;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitActionMethod(this);
    }

    internal abstract class Class<T>
    {
        internal abstract T Accept(IVisitor<T> visitor);
    }

    internal class CardClass<T> : Class<T>
    {
        internal Prop<T> _type;
        internal Prop<T> _name;
        internal Prop<T> _faction;
        internal Prop<T> _power;
        internal Prop<T> _range;
        internal Method<T> _onActivation;

        internal CardClass(Prop<T> type, Prop<T> name, Prop<T> faction, Prop<T> power, Prop<T> range, Method<T> onActivation)
        {
            _type = type;
            _name = name;
            _faction = faction;
            _power = power;
            _range = range;
            _onActivation = onActivation;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitCardClassClass(this);
    }

    internal class EffectClass<T> : Class<T>
    {
        internal Prop<T> _name;
        internal Method<T> _params;
        internal Method<T> _action;

        internal EffectClass(Prop<T> name, Method<T> Params, Method<T> action)
        {
            _name = name;
            _params = Params;
            _action = action;
        }

        internal override T Accept(IVisitor<T> visitor) => visitor.VisitEffectClassClass(this);
    }
}