using System;
using System.Data;
using System.IO;
using System.Text;

namespace CSharpConceptsDemo
{
    // 1️⃣ Enum Usage — represents user roles
    enum UserRole
    {
        Admin,
        Editor,
        Viewer
    }

    // 2️⃣ Class demonstrating Access Modifiers
    class Employee
    {
        // Public: accessible anywhere
        public string Name;

        // Private: accessible only inside this class
        private int salary;

        // Protected: accessible in this and derived classes
        protected int experience;

        // Internal: accessible within this assembly/project
        internal string Department;

        // Constructor
        public Employee(string name, int salary, int experience, string department)
        {
            Name = name;
            this.salary = salary;
            this.experience = experience;
            Department = department;
        }

        // Public method
        public void DisplayDetails()
        {
            Console.WriteLine($"Name: {Name}, Department: {Department}, Experience: {experience} years");
        }

        // Private helper method
        private void DisplaySalary()
        {
            Console.WriteLine($"Salary: {salary}");
        }

        // Public method calling private one
        public void ShowSalary()
        {
            DisplaySalary();
        }
    }

    // Derived class example (demonstrating protected)
    class Manager : Employee
    {
        public Manager(string name, int salary, int experience, string dept)
            : base(name, salary, experience, dept) { }

        public void Promote()
        {
            Console.WriteLine($"{Name} (Manager) promoted with {experience} years experience.");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("===== .NET Framework & C# Concepts Demo =====\n");

            // 3️⃣ Namespace & Library Usage (.NET library usage)

            Console.WriteLine($"Current Time: {DateTime.Now}");
            Console.WriteLine($"Square root of 25 = {Math.Sqrt(25)}");
            Console.WriteLine();

            // 4️⃣ Enum usage
            Console.WriteLine("Enum Demo:");
            UserRole role = UserRole.Editor;
            Console.WriteLine($"User role: {role}");
            Console.WriteLine("All roles: " + string.Join(", ", Enum.GetNames(typeof(UserRole))));
            Console.WriteLine();

            // 5️⃣ Access Modifiers Demo
            Console.WriteLine("Access Modifiers Demo:");
            Employee emp = new Employee("Alice", 50000, 5, "IT");
            emp.DisplayDetails();
            emp.ShowSalary();
            Console.WriteLine();

            Manager mgr = new Manager("Bob", 80000, 10, "HR");
            mgr.Promote();
            Console.WriteLine();

            // 6️⃣ Creating and Using DataTables
            Console.WriteLine("DataTable Demo:");
            DataTable table = new DataTable("Employees");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Age", typeof(int));

            table.Rows.Add(1, "Sahil", 25);
            table.Rows.Add(2, "Danish", 30);
            table.Rows.Add(3, "Hakim", 22);

            Console.WriteLine("All Records:");
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine($"{row["Id"]} | {row["Name"]} | {row["Age"]}");
            }

            // Filtering and Sorting
            Console.WriteLine("\nSorted by Age DESC:");
            DataRow[] sortedRows = table.Select("", "Age DESC");
            foreach (DataRow r in sortedRows)
                Console.WriteLine($"{r["Name"]} - {r["Age"]}");

            Console.WriteLine();

            // 7️ Methods for Manipulation
            Console.WriteLine("Methods for Manipulation Example:");
            string employeeSummary = GetEmployeeSummary(emp.Name, table.Rows.Count);
            Console.WriteLine(employeeSummary);
            Console.WriteLine();

            // 8️ Basic File Reading/Writing
            Console.WriteLine("File Read/Write Demo:");
            string filePath = "EmployeeData.txt";
            

            // Write data
            string fileContent = "Employee Report\n================\n";
            foreach (DataRow row in table.Rows)
            {
                fileContent += $"{row["Id"]}, {row["Name"]}, {row["Age"]}\n";
            }

            File.WriteAllText(filePath, fileContent);
            Console.WriteLine($"Data written to file: {filePath}");

            // Read data
            string readContent = File.ReadAllText(filePath);
            Console.WriteLine("\nFile Content:");
            Console.WriteLine(readContent);

            Console.WriteLine("\n===== END OF DEMO =====");
        }

        // Example method demonstrating manipulation
        static string GetEmployeeSummary(string name, int count)
        {
            return $"Employee '{name}' viewed total {count} records from the database.";
        }
    }
}
