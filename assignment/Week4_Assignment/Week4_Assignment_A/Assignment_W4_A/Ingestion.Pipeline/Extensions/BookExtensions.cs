using Assignment_W4_A.Ingestion.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Extensions
{
    /// <summary>Extension methods for sequences of <see cref="Book"/>.</summary>
    public static class BookExtensions
    {
        /// <summary>
        /// Returns the top <paramref name="n"/> elements ordered by the provided key selector (descending).
        /// </summary>
        public static IEnumerable<Book> TopBy<TValue>(this IEnumerable<Book> source, Func<Book, TValue> keySelector, int n)
            where TValue : IComparable
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (n <= 0)
            {
                return new Book[0];
            }

            List<Book> ordered = source.OrderByDescending(keySelector).Take(n).ToList();
            return ordered;
        }

        /// <summary>
        /// Produces counts grouped by Genre. Returns a dictionary genre->count.
        /// </summary>
        public static Dictionary<string, int> ToConditionCounts(this IEnumerable<Book> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Dictionary<string, int> result = source.GroupBy(b => string.IsNullOrWhiteSpace(b.Genre) ? "<Unknown>" : b.Genre)
                .ToDictionary(g => g.Key, g => g.Count());

            return result;
        }
    }

}
