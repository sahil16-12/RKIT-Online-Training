using Assignment_W4_A.Ingestion.Cli;
using Assignment_W4_A.Ingestion.Pipeline.Importers;
using Assignment_W4_A.Ingestion.Pipeline.Models;
using Assignment_W4_A.Ingestion.Pipeline.Summary;
using Assignment_W4_A.Ingestion.Pipeline.Writers;
using Assignment_W4_A.Ingestion.Pipeline.Extensions;    
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

/// <summary>Console application entry point that runs the ingestion pipeline.</summary>
class Program
{
    /// <summary>Main method.</summary>
    public static void Main(string[] args)
    {
        List<string> argsList = args.ToList();

        string input = Path.Combine(AppContext.BaseDirectory, "../../../Samples/in");
        string output = argsList.ElementAtOrDefault(1) ?? "./out";
        bool dryRun = argsList.Contains("--dry-run") || argsList.Contains("-n");

        Options opts = new Options(input, output, dryRun);

        Console.WriteLine(string.Format("Scanning: {0} (dry-run={1})", opts.InputDirectory, opts.DryRun));

        Directory.CreateDirectory(opts.OutputDirectory);

        Dictionary<string, FileImporter<Book>> importers = new Dictionary<string, FileImporter<Book>>(StringComparer.OrdinalIgnoreCase)
        {
            [".csv"] = new CsvBookImporter(),
            [".json"] = new JsonBookImporter()
        };

        List<Book> allBooks = new List<Book>();

        IEnumerable<string> files = Directory.EnumerateFiles(opts.InputDirectory, "*.*", SearchOption.AllDirectories).Where(f => importers.ContainsKey(Path.GetExtension(f)));

        foreach (string file in files)
        {
            string ext = Path.GetExtension(file);
            FileImporter<Book> importer = importers.GetValueOrDefault(ext);
            if (importer == null)
            {
                continue;
            }

            Console.WriteLine(string.Format("Importing {0} as {1}", file, importer.GetType().Name));

            try
            {
                IEnumerable<Book> items = importer.Import(file);
                foreach (Book b in items)
                {
                    allBooks.Add(b);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(string.Format("Failed to import {0}: {1}", file, ex.Message));
            }
        }

        Console.WriteLine(string.Format("Imported {0} books.", allBooks.Count));

        SummaryDto summary = new SummaryDto()
        {
            GeneratedAt = DateTime.UtcNow,
            TotalBooks = allBooks.Count,
            ByGenre = allBooks.ToConditionCounts(),
            TopPenaltyTitles = allBooks.TopBy(b => b.Penalty, 5).Select(b => b.Title).ToList()
        };

        string jsonOut = Path.Combine(opts.OutputDirectory, "summary.json");
        string xmlOut = Path.Combine(opts.OutputDirectory, "summary.xml");

        if (opts.DryRun)
        {
            Console.WriteLine("--dry-run specified; not writing files. Summary (json preview):");
            string preview = JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(preview);
        }
        else
        {
            string json = JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonOut, json);
            Console.WriteLine(string.Format("Wrote {0}", jsonOut));

            XmlReportWriter xmlWriter = new XmlReportWriter();
            xmlWriter.Write(xmlOut, new SummaryDto[] { summary });
            Console.WriteLine(string.Format("Wrote {0}", xmlOut));

            TextReportWriter textWriter = new TextReportWriter();
            string textPath = Path.Combine(opts.OutputDirectory, "summary.txt");
            textWriter.Write(textPath, allBooks);
            Console.WriteLine(string.Format("Wrote {0}", textPath));
        }

        Console.WriteLine("Done.");
    }
}
