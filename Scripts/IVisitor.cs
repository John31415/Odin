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
        T VisitBinaryExpr(Binary<T> expr);
        T VisitIndexExpr(Index<T> expr);
        T VisitCallExpr(Call<T> expr);
        T VisitGetExpr(Get<T> expr);
        T VisitSetExpr(Set<T> expr);
        T VisitGroupingExpr(Grouping<T> expr);
        T VisitLiteralExpr(Literal<T> expr);
        T VisitUnaryExpr(Unary<T> expr);
        T VisitVariableExpr(Variable<T> var);
        T VisitPostOperExpr(PostOper<T> oper);
        T VisitPreOperExpr(PreOper<T> oper);
        T VisitExpressionStmt(Expression<T> stmt);
        T VisitBlockStmt(Block<T> statements);
        T VisitVarStmt(Var<T> var);
        T VisitWhileStmt(While<T> statements);
        T VisitForStmt(For<T> statements);
        T VisitTypeProp(Type<T> prop);
        T VisitNameProp(Name<T> prop);
        T VisitFactionProp(Faction<T> prop);
        T VisitPowerProp(Power<T> prop);
        T VisitSourceProp(Source<T> prop);
        T VisitSingleProp(Single<T> prop);
        T VisitPredicateProp(Predicate<T> prop);
        T VisitParamValueProp(ParamValue<T> param);
        T VisitParamDeclProp(ParamDecl<T> param);
        T VisitRangeProp(Range<T> param);
        T VisitSelectorMethod(Selector<T> selector);
        T VisitEffectMethod(Effect<T> effect);
        T VisitOnActivationMethod(OnActivation<T> prop);
        T VisitOnActBodyMethod(OnActBody<T> prop);
        T VisitParamsMethod(Params<T> prop);
        T VisitActionMethod(Action<T> prop);
        T VisitCardClassClass(CardClass<T> prop);
        T VisitEffectClassClass(EffectClass<T> prop);
    }
}