using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaDemo
{
    // Simple Employee class
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Sample list of employees
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Employee> employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Alice", Salary = 50000 },
                new Employee { Id = 2, Name = "Bob", Salary = 75000 },
                new Employee { Id = 3, Name = "Charlie", Salary = 60000 },
                new Employee { Id = 4, Name = "Daisy", Salary = 90000 }
            };

            // 1. Filter employees who earn more than 60,000
            var highEarners = employees.Where(emp => emp.Salary > 60000);

            // 2. Sort these high earners by salary descending
            var sortedHighEarners = highEarners.OrderByDescending(emp => emp.Salary);

            // 3. Project to a simpler type (just name + salary)
            var summary = sortedHighEarners.Select(emp => new
            {
                emp.Name,
                emp.Salary
            });

            // 4. Print the result
            Console.WriteLine("High earners sorted by salary:");
            foreach (var item in summary)
            {
                Console.WriteLine($"{item.Name} — {item.Salary:C}");
                // {0:C} formats as currency
            }
        }
    }
}
