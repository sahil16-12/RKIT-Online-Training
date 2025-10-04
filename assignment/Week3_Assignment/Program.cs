using System.Data;
using Week3_Assignment.Domain;
using Week3_Assignment.IO;


string dateStr = DateTime.Now.ToString("yyyyMMdd");
string inputFile = $"returns_{dateStr}.csv";

if (!File.Exists(inputFile))
{
    string[] fileData =
    {
        "Id,Title,Author,Condition",
        "1,The Hobbit,J.R.R. Tolkien,Good",
        "2,1984,George Orwell,Damaged",
        "3,Dune,Frank Herbert,New",
        "4,The Catcher in the Rye,J.D. Salinger,Worn",
        "5,Animal Farm,George Orwell,Good"
    };

    File.WriteAllLines(inputFile, fileData);
    Console.WriteLine($"Sample CSV created: {inputFile}");
}


string outputDir = "./out";

try
{
    DataTable table = CsvReader.ReadCsvToTable(inputFile);
    List<Book> books = CsvReader.ConvertToBooks(table);

    ReportWriter.generateReport(books, outputDir, dateStr);

    Console.WriteLine("Report generated successfully!");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
catch (FormatException ex)
{
    Console.WriteLine($"CSV Format Error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}