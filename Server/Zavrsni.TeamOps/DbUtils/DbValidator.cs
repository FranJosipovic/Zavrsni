using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Zavrsni.TeamOps.DbUtils
{
    public static class DbValidator
    {
        public static async Task<bool> IsUnique<T, TValue>(
        DbContext dbContext,
        Expression<Func<T, TValue>> propertyExpression,
        TValue valueToCheck) where T : class
        {
            // Prepare the lambda expression for AnyAsync
            var parameter = Expression.Parameter(typeof(T), "entity");
            var body = Expression.Equal(propertyExpression.Body, Expression.Constant(valueToCheck));
            var lambda = Expression.Lambda<Func<T, bool>>(body, propertyExpression.Parameters);

            // Use the prepared lambda in AnyAsync to check for uniqueness
            return !await dbContext.Set<T>().AnyAsync(lambda);
        }
    }
}
