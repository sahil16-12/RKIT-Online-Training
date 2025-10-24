using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Models
{
    /// <summary>
    /// Represents a book record used across importers and report writers.
    /// Published uses <see cref="DateTime.MinValue"/> to indicate "unknown".
    /// </summary>
    public class Book
    {
        /// <summary>International Standard Book Number.</summary>
        public string ISBN { get; init; } = string.Empty;


        /// <summary>Human-readable title.</summary>
        public string Title { get; init; } = string.Empty;


        /// <summary>Author name.</summary>
        public string Author { get; init; } = string.Empty;


        /// <summary>Genre or category.</summary>
        public string Genre { get; init; } = string.Empty;


        /// <summary>Price in the store currency.</summary>
        public decimal Price { get; init; }


        /// <summary>Penalty value used as a sortable metric in examples.</summary>
        public decimal Penalty { get; init; }


        /// <summary>
        /// Publication date. If unknown, it will be <see cref="DateTime.MinValue"/>.
        /// </summary>
        public DateTime Published { get; init; } = DateTime.MinValue;


        /// <summary>
        /// Initializes a new instance of <see cref="Book"/>.
        /// </summary>
        public Book(string isbn, string title, string author, string genre, decimal price, decimal penalty, DateTime published)
        {
            ISBN = isbn ?? string.Empty;
            Title = title ?? string.Empty;
            Author = author ?? string.Empty;
            Genre = genre ?? string.Empty;
            Price = price;
            Penalty = penalty;
            Published = published;
        }


        /// <summary>Returns a short text representation of the book.</summary>
        public override string ToString()
        {
            string result = string.Format("{0} by {1} ({2})", Title, Author, ISBN);
            return result;
        }
    }
}
