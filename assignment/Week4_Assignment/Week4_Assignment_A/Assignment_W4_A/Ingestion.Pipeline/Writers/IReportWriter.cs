using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Writers
{

    /// <summary>
    /// Contract for writing report objects of type <typeparamref name="T"/> to disk.
    /// </summary>
    public interface IReportWriter<T>
    {
        /// <summary>Write the provided items to a file at <paramref name="path"/>.</summary>
        void Write(string path, IEnumerable<T> items);
    }

}
