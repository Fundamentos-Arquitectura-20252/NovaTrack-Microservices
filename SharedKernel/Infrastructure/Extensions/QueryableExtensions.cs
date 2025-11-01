using System.Linq.Expressions;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Pagination;


namespace SharedKernel.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            PagedRequest request,
            CancellationToken cancellationToken = default)
        {
            var totalCount = query.Count();
            
            var data = await Task.FromResult(query
                .Skip(request.Skip)
                .Take(request.PageSize)
                .ToList());

            return PagedResult<T>.Create(data, totalCount, request.Page, request.PageSize);
        }

        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> specification)
        {
            return query.Where(specification.ToExpression());
        }

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string? sortBy, bool descending = false)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = typeof(T).GetProperty(sortBy);
            
            if (property == null)
                return query;

            var propertyAccess = Expression.Property(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}