using Odin;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Console;
using static System.Math;

namespace Odin
{
    internal interface IVisitor<T>
    {
        internal T VisitBinaryExpr(Binary<T> expr);
        internal T VisitGroupingExpr(Grouping<T> expr);
        internal T VisitLiteralExpr(Literal<T> expr);
        internal T VisitUnaryExpr(Unary<T> expr);
        internal T VisitVariableExpr(Variable<T> var);
        internal T VisitExpressionStmt(Expression<T> stmt);
        internal T VisitVarStmt(Var<T> var);
    }
}