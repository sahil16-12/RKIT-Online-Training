using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaDemo
{

    class Program
    {
        static void Main(string[] args)
        {
            /// Sample list of employees
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Employee> employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Sahil", Salary = 50000 },
                new Employee { Id = 2, Name = "Danish", Salary = 75000 },
                new Employee { Id = 3, Name = "Hakim", Salary = 60000 },
                new Employee { Id = 4, Name = "Nahin", Salary = 90000 }
            };

            //// Have to add a function here, and see what we cant add in the arrow function
            
            //We cant add goto, continue or break that jumps out of lambda expression
            Func<Employee, decimal> calculateBonus = emp => emp.Salary * 0.10m;

            Console.WriteLine($"Employee {employees[0].Name} is having bonus { calculateBonus(employees[0])}");   

            // 1. Filter employees who earn more than 60,000
            IEnumerable<Employee> highEarners = employees.Where(emp => emp.Salary > 60000);

            // 2. Sort these high earners by salary descending
            IEnumerable<Employee> sortedHighEarners = highEarners.OrderByDescending(emp => emp.Salary);

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
