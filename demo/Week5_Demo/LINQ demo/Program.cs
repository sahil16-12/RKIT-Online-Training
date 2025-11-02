using LINQ_demo.Models;
using LINQ_demo.Services;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Demonstrates real-life LINQ queries with List and DataTable.
/// </summary>
public class Program
{
    public static void Main()
    {
        EmployeeService service = new EmployeeService();

        // Step 1: Load Data
        List<Employee> employees = service.GetEmployees();
        DataTable departments = service.GetDepartments();

        Console.WriteLine("=== All Employees ===");
        foreach (Employee e in employees)
        {
            Console.WriteLine($"{e.Id}: {e.Name} - DeptId: {e.DepartmentId}, Salary: {e.Salary}");
        }

        Console.WriteLine("\n=== Employees with Salary > 60000 ===");
        List<Employee> highEarners = service.GetHighSalaryEmployees(employees, 60000);
        foreach (Employee e in highEarners)
        {
            Console.WriteLine($"{e.Name} earns {e.Salary}");
        }

        Console.WriteLine("\n=== Employee - Department Join ===");
        IEnumerable<object> empDeptInfo = service.GetEmployeeDepartmentInfo(employees, departments);
        foreach (object item in empDeptInfo)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\n=== Average Salary by Department ===");
        IEnumerable<object> avgSalary = service.GetAverageSalaryByDepartment(employees, departments);
        foreach (object item in avgSalary)
        {
            Console.WriteLine(item);
        }
    }
}
