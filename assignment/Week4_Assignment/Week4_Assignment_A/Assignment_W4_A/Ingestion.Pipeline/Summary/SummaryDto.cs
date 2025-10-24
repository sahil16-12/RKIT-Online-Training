using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assignment_W4_A.Ingestion.Pipeline.Summary
{
    /// <summary>
    /// Represents a summary of imported books for reporting.
    /// </summary>
    public class SummaryDto
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalBooks { get; set; }

        [XmlIgnore]
        public Dictionary<string, int> ByGenre { get; set; } = new Dictionary<string, int>();

        [XmlArray("Genres")]
        public List<GenreCountEntry> ByGenreList
        {
            get
            {
                List<GenreCountEntry> list = new List<GenreCountEntry>();
                foreach (KeyValuePair<string, int> kvp in ByGenre)
                {
                    list.Add(new GenreCountEntry { Genre = kvp.Key, Count = kvp.Value });
                }
                return list;
            }
            set
            {
                ByGenre.Clear();
                if (value != null)
                {
                    foreach (GenreCountEntry entry in value)
                    {
                        ByGenre[entry.Genre] = entry.Count;
                    }
                }
            }
        }

        public List<string> TopPenaltyTitles { get; set; } = new List<string>();
    }

    /// <summary>
    /// Helper class for XML serialization of dictionary entries.
    /// </summary>
    public class GenreCountEntry
    {
        [XmlAttribute]
        public string Genre { get; set; }

        [XmlAttribute]
        public int Count { get; set; }
    }
}
