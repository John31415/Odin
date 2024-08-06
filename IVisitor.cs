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
        internal T visitBinaryExpr(Binary<T> expr);
        internal T visitGroupingExpr(Grouping<T> expr);
        internal T visitLiteralExpr(Literal<T> expr);
        internal T visitUnaryExpr(Unary<T> expr);
    }
}