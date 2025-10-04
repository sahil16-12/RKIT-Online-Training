using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3_Assignment.Domain;

namespace Week3_Assignment.IO
{
    internal static class ReportWriter
    {

        public static int getPenalty(BookCondition bookCondition)
        {
            return bookCondition switch
            {
                BookCondition.Damaged => 10,
                BookCondition.Worn => 20,
                BookCondition.New => -1,
                BookCondition.Good => 0,
            };
        }

        public static void generateReport(List<Book> books, string outputdir, string dateStr)
        {
            Directory.CreateDirectory(outputdir);

            var byCondition = new Dictionary<BookCondition, int>();

            foreach (var book in books)
            {
                if (byCondition.ContainsKey(book.Condition))
                {
                    byCondition[book.Condition]++;
                }
                else
                {
                    byCondition[book.Condition] = 1;
                }
            }

            var bookPenalties = new List<(Book Book, int Penalty)>();

            foreach (var book in books)
            {
                int rawPenalty = getPenalty(book.Condition);
                int safePenalty = Math.Clamp(rawPenalty, 0, 100);
                bookPenalties.Add((book, safePenalty));
            }

            var top5 = bookPenalties
                .OrderByDescending(x => x.Penalty)
                .Take(5)
                .ToList();

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string report = $"Date/time processed: {timestamp}\n" +
                            $"Total returns: {books.Count}\n\n" +
                            "Count by condition:\n";

            foreach (var kvp in byCondition)
                report += $"- {kvp.Key}: {kvp.Value}\n";

            report += "\nTop 5 titles by penalty:\n";
            foreach (var item in top5)
                report += $"{item.Book.Title} ({item.Penalty})\n";

            string outputPath = Path.Combine(outputdir, $"daily_summary_{dateStr}.txt");
            File.WriteAllText(outputPath, report);
        }
    }
}
