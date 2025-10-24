using Assignment_W4_A.Ingestion.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Importers
{
    /// <summary>
    /// JSON importer that expects the JSON file to contain an array of <see cref="Book"/> objects.
    /// Sealed to keep the importer focused on this JSON shape.
    /// </summary>
    public sealed class JsonBookImporter : FileImporter<Book>
    {
        /// <summary>Imports books from the JSON file at <paramref name="path"/>.</summary>
        public override IEnumerable<Book> Import(string path)
        {
            if (!File.Exists(path))
            {
                yield break;
            }

            string json = File.ReadAllText(path);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Book>? books = JsonSerializer.Deserialize<List<Book>>(json, options);

            if (books == null)
            {
                yield break;
            }

            foreach (Book book in books)
            {
                yield return book;
            }
        }
    }

}
