using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_using_ormlite_demo.Models
{
    /// <summary>
    /// Represents an employee record in the database.
    /// </summary>
    public class Employee
    {
        [AutoIncrement] // Automatically increments the ID
        [PrimaryKey]
        public int Id { get; set; }

        [Required] // Ensures this field cannot be null
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }

        [Required]
        public decimal Salary { get; set; }
    }
}
