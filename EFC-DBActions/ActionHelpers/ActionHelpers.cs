using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFC_Interfaces;

namespace EFC_DBActions.ActionHelpers
{
    public static class ActionHelpers
    {

        public static Task<Expression<Func<TEntity, TJoinEntity, object>>> BuildJoinLambdaExpression<TEntity, TJoinEntity>(IEnumerable<string>? outerGetFields, IEnumerable<string>? innerGetFields)
            where TEntity : class, IEFModel, new()
            where TJoinEntity : class, IEFModel, new()
        {
            var parameter1 = Expression.Parameter(typeof(TEntity), "x");
            var parameter2 = Expression.Parameter(typeof(TJoinEntity), "y");

            IEnumerable<MemberAssignment>? bindings = [];

            if (outerGetFields is not null || outerGetFields!.Any())
            {
                bindings = outerGetFields!.Select(field =>
                {
                    var property = typeof(TEntity).GetProperty(field) ?? throw new ArgumentNullException($"Property {field} does not exist on type {typeof(TEntity).Name}");

                    return Expression.Bind(property, Expression.Property(parameter1, property));
                });
            }

            if (innerGetFields is not null || innerGetFields!.Any())
            {
                bindings = bindings.Concat(innerGetFields!.Select(field =>
                {
                    var property = typeof(TJoinEntity).GetProperty(field) ?? throw new ArgumentNullException($"Property {field} does not exist on type {typeof(TJoinEntity).Name}");

                    return Expression.Bind(property, Expression.Property(parameter2, property));
                }));
            }

            var body = Expression.MemberInit(Expression.New(typeof(object)), bindings);

            return Task.FromResult(Expression.Lambda<Func<TEntity, TJoinEntity, object>>(body, parameter1, parameter2));
        }
    }
}