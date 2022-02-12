using Employee.DTO.Common;

namespace Employee.DTO.Employee.Employee
{
    public class EmployeeDto : BaseEntityDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
