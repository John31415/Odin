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
            _var= var;
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
        internal Var(Token name, Token type,Expr<T> initializer)
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
}