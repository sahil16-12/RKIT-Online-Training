using System;
using System.Collections.Generic;
using System.Linq;

namespace DelegateDemo
{
    // 🔹 Custom delegate: takes a string message, returns void
    public delegate void Notify(string message);

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // 👩‍💼 Sample employee list
            List<Employee> employees = new()
            {
                new Employee { Id = 1, Name = "Alice", Salary = 50000 },
                new Employee { Id = 2, Name = "Bob", Salary = 65000 },
                new Employee { Id = 3, Name = "Charlie", Salary = 45000 },
                new Employee { Id = 4, Name = "Daisy", Salary = 80000 }
            };

            // 🟢 1. Custom Delegate — used for logging or notifications
            Notify notify = msg => Console.WriteLine($"[LOG] {msg}");
            notify("Employee system started...");

            // 🟢 2. Predicate — test condition (who earns more than 50k)
            Predicate<Employee> isHighEarner = emp => emp.Salary > 50000;
            var highEarners = employees.FindAll(isHighEarner);

            notify("Filtered employees earning more than 50,000:");

            // 🟢 3. Action — print details (no return)
            Action<Employee> printEmployee = emp =>
                Console.WriteLine($"{emp.Name} earns ₹{emp.Salary:N0}");

            highEarners.ForEach(printEmployee); // Call Action on each employee

            // 🟢 4. Func — calculate yearly bonus and return a result
            Func<Employee, decimal> calculateBonus = emp => emp.Salary * 0.10m;

            Console.WriteLine("\nYearly Bonus Details:");
            foreach (var emp in highEarners)
            {
                decimal bonus = calculateBonus(emp);
                Console.WriteLine($"{emp.Name}'s bonus = ₹{bonus:N0}");
            }

            // 🟢 Combine Action + Func: Display formatted results
            Action<Employee> displayFullDetails = emp =>
            {
                var bonus = calculateBonus(emp);
                Console.WriteLine($"👤 {emp.Name} | Salary: ₹{emp.Salary:N0} | Bonus: ₹{bonus:N0}");
            };

            Console.WriteLine("\nFull Details (using combined Action + Func):");
            highEarners.ForEach(displayFullDetails);

            notify("Process completed successfully!");
        }
    }
}
