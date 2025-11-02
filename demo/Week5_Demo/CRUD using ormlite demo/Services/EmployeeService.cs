using System;
using System.Collections.Generic;
using ServiceStack.OrmLite;
using System.Data;
using CRUD_using_ormlite_demo.Models;
using ServiceStack.OrmLite.MySql;
using ServiceStack.Data;

namespace CRUD_using_ormlite_demo.Services
{
    /// <summary>
    /// Provides CRUD operations for Employee entities using ServiceStack.OrmLite.
    /// </summary>
    public class EmployeeService
    {
        private readonly IDbConnectionFactory _dbFactory;

        /// <summary>
        /// Initializes a new instance of EmployeeService with a given connection factory.
        /// </summary>
        /// <param name="dbFactory">The database connection factory for creating connections.</param>
        public EmployeeService(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// Creates the Employee table if it doesn't exist.
        /// </summary>
        public void InitializeDatabase()
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                db.CreateTableIfNotExists<Employee>();
            }
        }

        /// <summary>
        /// Inserts a new employee record into the database.
        /// </summary>
        public void AddEmployee(Employee employee)
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                long returnedId = db.Insert(employee, selectIdentity: true); // Insert and get the generated Id
                Console.WriteLine($"Inserted Employee with ID: {returnedId}");
            }
        }
        /// <summary>
        /// Inserts multiple employees into the database.
        /// </summary>
        public void AddEmployees(IEnumerable<Employee> employees)
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                db.InsertAll(employees);
            }
        }
        /// <summary>
        /// Retrieves all employees from the database.
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                List<Employee> employees = db.Select<Employee>();
                return employees;
            }
        }

        /// <summary>
        /// Updates the salary of an employee.
        /// </summary>
        public void UpdateEmployeeSalary(int employeeId, decimal newSalary)
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                Employee? employee = db.SingleById<Employee>(employeeId);
                if (employee != null)
                {
                    employee.Salary = newSalary;
                    db.Update(employee);
                }
                else
                {
                    Console.WriteLine($"Employee with ID {employeeId} not found.");
                }
            }
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        public void DeleteEmployee(int employeeId)
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                db.DeleteById<Employee>(employeeId);
            }
        }
    }
}
