using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionBuilder
{
    public class ExpressionBuilder<TEntity>
    {
        public Expression<Func<TEntity, bool>> Expression { get; private set; }

        public void Append(Expression<Func<TEntity, bool>> expressionPiece)
        {
            if (Expression == null)
            {
                Expression = expressionPiece;
            }
            else
            {
                var map = Expression.Parameters.Select((f, i) => new { f, s = expressionPiece.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
                var invokedExpression = ParameterRebinder.ReplaceParameters(map, expressionPiece.Body);
                Expression = System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>
                      (System.Linq.Expressions.Expression.AndAlso(Expression.Body, invokedExpression), Expression.Parameters);
            }
        }

        class ParameterRebinder : ExpressionVisitor
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
                if (map.TryGetValue(p, out ParameterExpression replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }
}
