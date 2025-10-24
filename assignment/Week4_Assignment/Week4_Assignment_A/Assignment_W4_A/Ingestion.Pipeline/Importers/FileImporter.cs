using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Importers
{
    /// <summary>
    /// Abstract base class that defines an importer contract for type <typeparamref name="T"/>.
    /// Concrete importers should inherit and implement <see cref="Import"/>.
    /// </summary>
    public abstract class FileImporter<T>
    {
        /// <summary>
        /// Import items from the specified file path.
        /// Implementations decide how to interpret the path and perform parsing.
        /// </summary>
        /// <param name="path">Path to the file to import.</param>
        /// <returns>Sequence of items of type <typeparamref name="T"/>.</returns>
        public abstract IEnumerable<T> Import(string path);
    }
}
