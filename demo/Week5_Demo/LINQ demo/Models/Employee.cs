using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_demo.Models
{
    /// <summary>
    /// Represents an employee in the company.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Unique ID of the employee.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the employee.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Department ID to which the employee belongs.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Employee's salary in INR.
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Constructor to initialize an Employee object.
        /// </summary>
        public Employee(int id, string name, int departmentId, decimal salary)
        {
            Id = id;
            Name = name;
            DepartmentId = departmentId;
            Salary = salary;
        }
    }

}
