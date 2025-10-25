# Assignment_W4_A

Comprehensive README for the Assignment_W4_A project.

## Summary

This solution is a small ingestion pipeline demo implemented in .NET (C#). It shows how to discover and import book data from different sources (CSV, JSON, files), run a small ingestion CLI, and write simple reports (text or XML). The project was structured to demonstrate extension points (importers, writers, plugin discovery) and to keep components separated for testability and reusability.

Directory: `Assignment_W4_A`

## Project structure

- `Ingestion.Cli/` — Console app exposing the ingestion CLI (entry point).
- `Ingestion.Pipeline/` — Core pipeline code, importers, models, writers and plugin discovery.
  - `Collections/` — helper collections like `BookSet`.
  - `Extensions/` — extension methods such as `BookExtensions`.
  - `Importers/` — different importers (CSV, JSON, generic file importer).
  - `Models/` — `Book` model and related types.
  - `Plugins/` — importer discovery utilities.
  - `Summary/` — DTOs for pipeline summaries (e.g. `SummaryDto`).
  - `Writers/` — report writers (interfaces and implementations for text and XML).
- `Samples/in/` — example sample inputs used by the CLI (CSV and JSON files).

## Goals / Contract

- Input: a folder or file containing book data in one of the supported formats (CSV, JSON, etc.).
- Output: generated reports in text and/or XML formats via writer plugins.
- Error modes: invalid input files should be reported, invalid records should be skipped or reported (pipeline should continue where possible).

Success criteria: CLI runs successfully, imports book entries from sample files, and writes report file(s) to an output location.

## Build

From the `Assignment_W4_A` folder (project root), build all projects:

```powershell
# From the Assignment_W4_A folder
cd c:\Users\sahil\OneDrive\Desktop\Rkit-Online-Training\assignment\Week4_Assignment\Week4_Assignment_A\Assignment_W4_A
dotnet build
```

## Run (CLI)

The CLI project is `Ingestion.Cli`. You can run it using `dotnet run`

Example (run directly):

```powershell
# Run the CLI, pointing it at the included sample input folder and a new output directory
dotnet run
```

## Sample usage scenarios

- Ingest all sample files from `Samples/in/` and write reports to `out` folder:

```powershell
dotnet run
```

- Run using --dry-run option:

```powershell
dotnet run -- --dry-run
```

After running, check the `out` directory for generated reports. The pipeline includes `TextReportWriter` and `XmlReportWriter` implementations, so expect at least `.txt` and/or `.xml` outputs depending on configuration.

## What the pipeline does (high-level)

1.  Discover available importers (via `Plugins/ImporterDiscovery`).
2.  Use importer(s) to parse input files into `Book` model instances.
3.  Aggregate results into a `SummaryDto`.
4.  Use `Writers` to produce report files (text and XML writers provided).

## License & Author

Author: Sahil (repository owner)

This README was generated to document the assignment project. Use and adapt it as needed for submission or further development.
