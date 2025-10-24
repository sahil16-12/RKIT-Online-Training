using Assignment_W4_A.Ingestion.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Writers
{
    /// <summary>
    /// Writes a human-readable plain text summary for a sequence of <see cref="Book"/> objects.
    /// Sealed to encourage composition over inheritance for alternate text formats.
    /// </summary>
    public sealed class TextReportWriter : IReportWriter<Book>
    {
        /// <summary>Writes a plain-text summary to the given file path.</summary>
        public void Write(string path, IEnumerable<Book> items)
        {
            List<Book> list = items.ToList();

            List<string> lines = new List<string>();

            lines.Add("Book Import Summary");
            lines.Add("===================");
            lines.Add(string.Format("TotalBooks: {0}", list.Count));

            IEnumerable<IGrouping<string, Book>> byGenre = list.GroupBy(b => string.IsNullOrWhiteSpace(b.Genre) ? "<Unknown>" : b.Genre).OrderByDescending(g => g.Count());
            lines.Add("By Genre:");
            foreach (IGrouping<string, Book> group in byGenre)
            {
                lines.Add(string.Format("  {0}: {1}", group.Key, group.Count()));
            }

            IEnumerable<Book> topPenalty = list.OrderByDescending(b => b.Penalty).Take(5);
            lines.Add("Top penalty books:");
            foreach (Book b in topPenalty)
            {
                lines.Add(string.Format("  {0} ({1:C})", b.Title, b.Penalty));
            }

            File.WriteAllLines(path, lines);
        }
    }

}
