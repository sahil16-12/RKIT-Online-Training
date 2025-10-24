using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Cli
{
    /// <summary>Simple options record for the CLI host.</summary>
    public record Options(string InputDirectory, string OutputDirectory, bool DryRun = false);
}
