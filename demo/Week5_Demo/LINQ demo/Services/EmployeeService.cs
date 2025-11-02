using LINQ_demo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_demo.Services
{
    /// <summary>
    /// Provides functionality to perform operations on employees using LINQ.
    /// </summary>
    public class EmployeeService
    {
        /// <summary>
        /// Returns a list of employees (simulating data from a database).
        /// </summary>
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>
        {
            new Employee(1, "Sahil", 101, 65000),
            new Employee(2, "Hakim", 102, 55000),
            new Employee(3, "Sameed", 101, 80000),
            new Employee(4, "Nahin", 103, 45000),
            new Employee(5, "Danish", 102, 90000)
        };
            return employees;
        }

        /// <summary>
        /// Creates a DataTable containing department information.
        /// </summary>
        public DataTable GetDepartments()
        {
            DataTable departmentTable = new DataTable("Departments");
            departmentTable.Columns.Add("DepartmentId", typeof(int));
            departmentTable.Columns.Add("DepartmentName", typeof(string));

            departmentTable.Rows.Add(101, "IT");
            departmentTable.Rows.Add(102, "HR");
            departmentTable.Rows.Add(103, "Finance");

            return departmentTable;
        }

        /// <summary>
        /// Finds all employees having salary greater than a given threshold.
        /// </summary>
        public List<Employee> GetHighSalaryEmployees(List<Employee> employees, decimal minSalary)
        {
            //List<Employee> highEarners = (from e in employees where e.Salary > minSalary orderby e.Salary descending select e).ToList();
            List<Employee> highEarners = employees
                .Where(e => e.Salary > minSalary)
                .OrderByDescending(e => e.Salary)
                .ToList();

            return highEarners;
        }

        /// <summary>
        /// Performs a join between employees and departments.
        /// </summary>
        public IEnumerable<object> GetEmployeeDepartmentInfo(List<Employee> employees, DataTable departments)
        {
            var query = employees
                .Join(
                    departments.AsEnumerable(),
                    e => e.DepartmentId,
                    d => d.Field<int>("DepartmentId"),
                    (e, d) => new
                    {
                        EmployeeName = e.Name,
                        Department = d.Field<string>("DepartmentName"),
                        Salary = e.Salary
                    }
                )
                .ToList();

            return query;
        }

        /// <summary>
        /// Calculates the average salary of employees in each department.
        /// </summary>
        public IEnumerable<object> GetAverageSalaryByDepartment(List<Employee> employees, DataTable departments)
        {
            var result = employees
                .GroupBy(e => e.DepartmentId)
                .Join(
                    departments.AsEnumerable(),
                    g => g.Key,
                    d => d.Field<int>("DepartmentId"),
                    (g, d) => new
                    {
                        Department = d.Field<string>("DepartmentName"),
                        AverageSalary = g.Average(x => x.Salary)
                    }
                )
                .ToList();

            return result;
        }

    }
}
