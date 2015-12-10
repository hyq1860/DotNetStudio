using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common
{
   public static class LambdaHelper
    {
        /// <summary>
        /// 合并表达式树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            var p = Expression.Parameter(typeof(T), "x");
            var bd = Expression.AndAlso(
                    Expression.Invoke(a, p),
                    Expression.Invoke(b, p));
            var ld = Expression.Lambda<Func<T, bool>>(bd, p);
            return ld;
        }
    }

   public static class PredicateBuilder
   {

       public static Expression<Func<T, bool>> True<T>() { return f => true; }
       public static Expression<Func<T, bool>> False<T>() { return f => false; }
       public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
       {
           // build parameter map (from parameters of second to parameters of first)
           var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

           // replace parameters in the second lambda expression with parameters from the first
           var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

           // apply composition of lambda expression bodies to parameters from the first expression 
           return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
       }

       public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
       {
           return first.Compose(second, Expression.And);
       }

       public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
       {
           return first.Compose(second, Expression.Or);
       }
   }
   public class ParameterRebinder : ExpressionVisitor
   {
       private readonly Dictionary<ParameterExpression, ParameterExpression> map;

       public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
       {
           this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
       }

       public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
       {
           return new ParameterRebinder(map).Visit(exp);
       }

       protected override Expression VisitParameter(ParameterExpression p)
       {
           ParameterExpression replacement;
           if (map.TryGetValue(p, out replacement))
           {
               p = replacement;
           }
           return base.VisitParameter(p);
       }
   }
}
