namespace MagicVilla_API.Extentions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies pagination to an IQueryable collection by skipping a specified number of items and taking a specified number of items.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="query">The IQueryable collection to paginate.</param>
        /// <param name="pageSize">The number of items to include per page.</param>
        /// <param name="pageNumber">The page number (1-based index) of the desired page.</param>
        /// <returns>The paginated IQueryable collection.</returns>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int pageSize, int pageNumber)
        {
            // Calculate the number of items to skip and take based on page size and number.
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
