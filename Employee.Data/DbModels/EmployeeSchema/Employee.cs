using Employee.Data.BaseModeling;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Data.DbModels.EmployeeSchema
{
    [Table("Employees", Schema = "Employee")]
    public class Employee : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
