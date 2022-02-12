using Employee.DTO.Common;

namespace Employee.DTO.Employee.Employee
{
    public class EmployeeFilterDto : BaseFilterDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
