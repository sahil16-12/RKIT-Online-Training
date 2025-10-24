using Assignment_W4_A.Ingestion.Pipeline.Summary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assignment_W4_A.Ingestion.Pipeline.Writers
{
    /// <summary>
    /// Serializes <see cref="SummaryDto"/> instances to XML using <see cref="XmlSerializer"/>.
    /// Sealed to keep the behaviour explicit and testable.
    /// </summary>
    public sealed class XmlReportWriter : IReportWriter<SummaryDto>
    {
        /// <summary>Writes the provided summary items as an XML list to <paramref name="path"/>.</summary>
        public void Write(string path, IEnumerable<SummaryDto> items)
        {
            List<SummaryDto> list = items is List<SummaryDto> l ? l : new List<SummaryDto>(items);
            XmlSerializer serializer = new XmlSerializer(typeof(List<SummaryDto>));
            using FileStream fs = File.Create(path);
            serializer.Serialize(fs, list);
        }
    }
}
