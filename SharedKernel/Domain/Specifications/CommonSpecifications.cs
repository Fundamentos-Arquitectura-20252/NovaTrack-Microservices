using System.Linq.Expressions;



namespace SharedKernel.Domain.Specifications
{
    public class ActiveEntitySpecification<T> : Specification<T> where T : class
    {
        public override Expression<Func<T, bool>> ToExpression()
        {
            var property = typeof(T).GetProperty("IsActive");
            if (property == null)
                return entity => true;

            var parameter = Expression.Parameter(typeof(T));
            var propertyAccess = Expression.Property(parameter, property);
            var constant = Expression.Constant(true);
            var equals = Expression.Equal(propertyAccess, constant);

            return Expression.Lambda<Func<T, bool>>(equals, parameter);
        }
    }

    public class CreatedInDateRangeSpecification<T> : Specification<T> where T : class
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public CreatedInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var property = typeof(T).GetProperty("CreatedAt");
            if (property == null)
                return entity => true;

            var parameter = Expression.Parameter(typeof(T));
            var propertyAccess = Expression.Property(parameter, property);
            
            var startConstant = Expression.Constant(_startDate);
            var endConstant = Expression.Constant(_endDate);
            
            var greaterThanOrEqual = Expression.GreaterThanOrEqual(propertyAccess, startConstant);
            var lessThanOrEqual = Expression.LessThanOrEqual(propertyAccess, endConstant);
            var andExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
        }
    }
}