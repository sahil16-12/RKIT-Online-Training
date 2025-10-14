using NewtonSoftDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewtonSoftDemo.Services
{
    // Handles all JSON-related operations
    public static class JsonService
    {
        // Converts an object (like List<Employee>) to JSON string
        public static string SerializeEmployees(List<Employee> employees)
        {
            // Formatting.Indented makes JSON more readable
            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }

        // Converts JSON string back to List<Employee>
        public static List<Employee> DeserializeEmployees(string json)
        {
            return JsonConvert.DeserializeObject<List<Employee>>(json);
        }

        // Writes JSON data to file
        public static void SaveToFile(string json, string filePath)
        {
            File.WriteAllText(filePath, json);
        }

        // Reads JSON from file and returns content
        public static string ReadFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        // Demonstrates reading JSON dynamically using JObject
        public static void PrintJsonSummary(string json)
        {
            var employeesArray = JArray.Parse(json);
            Console.WriteLine("\nEmployee Summary (via JObject):");

            foreach (var emp in employeesArray)
            {
                string name = emp["Name"]?.ToString();
                string dept = emp["Department"]?.ToString();
                double salary = emp["Salary"]?.ToObject<double>() ?? 0;

                Console.WriteLine($"- {name} ({dept}) → ₹{salary:N0}");
            }
        }
    }
}
