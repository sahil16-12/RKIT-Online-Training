// Initialize sample employee data
using NewtonSoftDemo.Models;
using NewtonSoftDemo.Services;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Sahil", Department = "HR", Salary = 50000 },
                new Employee { Id = 2, Name = "Hakim", Department = "IT", Salary = 70000 },
                new Employee { Id = 3, Name = "Danish", Department = "Finance", Salary = 65000 }
            };

// Serialize to JSON
string jsonData = JsonService.SerializeEmployees(employees);
Console.WriteLine("=== Serialized JSON ===");
Console.WriteLine(jsonData);

// Save JSON to file
string filePath = "employees.json";
JsonService.SaveToFile(jsonData, filePath);

// Read JSON back from file
string readData = JsonService.ReadFromFile(filePath);

// Deserialize JSON to C# objects
var deserializedEmployees = JsonService.DeserializeEmployees(readData);
Console.WriteLine("\n=== Deserialized Objects ===");
foreach (var emp in deserializedEmployees)
    Console.WriteLine($"{emp.Id}. {emp.Name} ({emp.Department}) - ₹{emp.Salary}");

// Demonstrate JObject usage
JsonService.PrintJsonSummary(readData);