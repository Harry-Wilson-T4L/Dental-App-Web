using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DentalDrill.CRM.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereOneOf<T>(this IQueryable<T> query, IEnumerable<Expression<Func<T, Boolean>>> predicates)
        {
            var predicatesList = predicates.ToList();
            if (predicatesList.Count == 0)
            {
                return query.Where(x => false);
            }

            if (predicatesList.Count == 1)
            {
                return query.Where(predicatesList[0]);
            }

            var parameter = Expression.Parameter(typeof(T), "x");

            var body = Expression.OrElse(
                predicatesList[0].Body.ReplaceParameter(predicatesList[0].Parameters[0], parameter),
                predicatesList[1].Body.ReplaceParameter(predicatesList[1].Parameters[0], parameter));
            for (var i = 2; i < predicatesList.Count; i++)
            {
                body = Expression.OrElse(
                    body,
                    predicatesList[i].Body.ReplaceParameter(predicatesList[i].Parameters[0], parameter));
            }

            var combinedPredicate = Expression.Lambda<Func<T, Boolean>>(body, parameter);
            return query.Where(combinedPredicate);
        }

        private static Expression ReplaceParameter(this Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression oldParameter;
            private readonly ParameterExpression newParameter;

            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                this.oldParameter = oldParameter;
                this.newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (Object.Equals(node, this.oldParameter))
                {
                    return this.newParameter;
                }

                return this.oldParameter;
            }
        }
    }
}
