using System.Linq.Expressions;

// Pagination Models
namespace SharedKernel.Infrastructure.Pagination
{

    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
        public int? PreviousPage => HasPreviousPage ? Page - 1 : null;
        public int? NextPage => HasNextPage ? Page + 1 : null;

        public static PagedResult<T> Create(IEnumerable<T> data, int totalCount, int page, int pageSize)
        {
            return new PagedResult<T>
            {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
