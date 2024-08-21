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
        internal T VisitPostOperExpr(PostOper<T> oper);
        internal T VisitPreOperExpr(PreOper<T> oper);
        internal T VisitExpressionStmt(Expression<T> stmt);
        internal T VisitBlockStmt(Block<T> statements);
        internal T VisitVarStmt(Var<T> var);
        internal T VisitWhileStmt(While<T> statements);
        internal T VisitForStmt(For<T> statements);
        internal T VisitTypeProp(Type<T> prop);
        internal T VisitNameProp(Name<T> prop);
        internal T VisitFactionProp(Faction<T> prop);
        internal T VisitPowerProp(Power<T> prop);
        internal T VisitSourceProp(Source<T> prop);
        internal T VisitSingleProp(Single<T> prop);
        internal T VisitPredicateProp(Predicate<T> prop);
        internal T VisitParamValueProp(ParamValue<T> param);
        internal T VisitParamDeclProp(ParamDecl<T> param);
        internal T VisitRangeProp(Range<T> param);
        internal T VisitSelectorMethod(Selector<T> selector);
        internal T VisitEffectMethod(Effect<T> effect);
        internal T VisitOnActivationMethod(OnActivation<T> prop);
        internal T VisitOnActBodyMethod(OnActBody<T> prop);
        internal T VisitParamsMethod(Params<T> prop);
        internal T VisitActionMethod(Action<T> prop);
        internal T VisitCardClassClass(CardClass<T> prop);
        internal T VisitEffectClassClass(EffectClass<T> prop);
    }
}