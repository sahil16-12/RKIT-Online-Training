using Assignment_W4_A.Ingestion.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Importers
{
    /// <summary>
    /// CSV importer for <see cref="Book"/> records.
    /// Sealed because the CSV parsing policy is self-contained and should not be extended by inheritance.
    /// </summary>
    public sealed class CsvBookImporter : FileImporter<Book>
    {
        private readonly char _separator;

        /// <summary>Creates a new CSV importer with an optional separator (default is comma).</summary>
        public CsvBookImporter(char separator)
        {
            _separator = separator;
        }

        /// <summary>Creates a new CSV importer with default separator ','.</summary>
        public CsvBookImporter() : this(',')
        {
        }

        /// <summary>Imports books from the CSV file at <paramref name="path"/>.</summary>
        public override IEnumerable<Book> Import(string path)
        {
            if (!File.Exists(path))
            {
                yield break;
            }

            using StreamReader reader = new StreamReader(path);

            string headerLine = reader.ReadLine() ?? string.Empty;

            while (reader.EndOfStream == false)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                IEnumerable<string> splitted = SplitCsvLine(line, _separator);
                List<string> parts = splitted.ToList();

                string isbn = parts.ElementAtOrDefault(0) ?? string.Empty;
                string title = parts.ElementAtOrDefault(1) ?? string.Empty;
                string author = parts.ElementAtOrDefault(2) ?? string.Empty;
                string genre = parts.ElementAtOrDefault(3) ?? string.Empty;

                decimal price = 0m;
                decimal.TryParse(parts.ElementAtOrDefault(4) ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture, out price);

                decimal penalty = 0m;
                decimal.TryParse(parts.ElementAtOrDefault(5) ?? string.Empty, NumberStyles.Any, CultureInfo.InvariantCulture, out penalty);

                DateTime published = DateTime.MinValue;
                DateTime.TryParse(parts.ElementAtOrDefault(6) ?? string.Empty, CultureInfo.InvariantCulture, DateTimeStyles.None, out published);

                Book book = new Book(isbn, title, author, genre, price, penalty, published);
                yield return book;
            }
        }

        /// <summary>
        /// Splits a CSV line into fields handling quoted fields with separators inside.
        /// </summary>
        private static IEnumerable<string> SplitCsvLine(string line, char sep)
        {
            string current = string.Empty;
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];
                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (ch == sep && inQuotes == false)
                {
                    yield return current.Trim();
                    current = string.Empty;
                    continue;
                }

                current += ch;
            }

            yield return current.Trim();
        }
    }

}


