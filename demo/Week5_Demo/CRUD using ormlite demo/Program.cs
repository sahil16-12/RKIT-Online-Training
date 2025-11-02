using CRUD_using_ormlite_demo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json; 
using ServiceStack.OrmLite;
using System.Configuration;
using CRUD_using_ormlite_demo.Services;

Console.WriteLine("==== Employee Management System ====");

// Load configuration from appsettings.json
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string? connectionString = config.GetConnectionString("MySqlConnection");

 // Initialize database connection
OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);
EmployeeService employeeService = new EmployeeService(dbFactory);

// Create table if not exists
employeeService.InitializeDatabase();

// Create new employee record
Employee newEmployee = new Employee
{
    Name = "Shahil Vora",
    Department = "Engineering",
    Salary = 80000m
};

Employee secondEmployee = new Employee
{
    Name = "Dave Nahin",
    Department = "Engineering",
    Salary = 56000m
};

employeeService.AddEmployee(newEmployee);

Console.WriteLine("Employee added successfully.");

employeeService.AddEmployees(new List<Employee> { newEmployee, secondEmployee });

// Read all employees
Console.WriteLine("\nAll Employees:");
foreach (Employee emp in employeeService.GetAllEmployees())
{
    Console.WriteLine($"{emp.Id} - {emp.Name} - {emp.Department} - ${emp.Salary}");
}

// Update salary
employeeService.UpdateEmployeeSalary(1, 95000m);
Console.WriteLine("\nEmployee salary updated successfully.");

// Delete employee
employeeService.DeleteEmployee(1);
Console.WriteLine("\nEmployee deleted successfully.");

Console.WriteLine("\n==== End of Demo ====");