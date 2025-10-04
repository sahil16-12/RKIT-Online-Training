using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3_Assignment.Domain;

namespace Week3_Assignment.IO
{
    internal static class CsvReader
    {

        // Filepath will containg the path of the csv file.
        public static DataTable ReadCsvToTable(string filePath)
        {
            if(!File.Exists(filePath))
                throw new FileNotFoundException("Input file not found", filePath);

            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("Condition", typeof(string));

            using var reader = new StreamReader(filePath);
            string? header = reader.ReadLine(); // Skip the column names row

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                    continue;

                string[] parts = line.Split(',');

                if (parts.Length != 4)
                    throw new FormatException($"Invalid row format: {line}");

                table.Rows.Add(int.Parse(parts[0]), parts[1], parts[2], parts[3]);

            }

            return table;
        }


        // table will contain our table created from csv file
        public static List<Book> ConvertToBooks(DataTable table)
        {
            var list = new List<Book>();
            foreach (DataRow row in table.Rows)
            {

                if (!Enum.TryParse(row["Condition"].ToString(), out BookCondition condition))
                    throw new ArgumentException($"Invalid condition: {row["Condition"]}");

                list.Add(new Book(
                    Convert.ToInt32(row["Id"]),
                    row["Title"].ToString() ?? "",
                    row["Author"].ToString() ?? "",
                    condition
                    ));
            }
            return list;



        }
    }
}
