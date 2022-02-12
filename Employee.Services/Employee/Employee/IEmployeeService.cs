using Employee.DTO.Common;
using Employee.DTO.Employee.Employee;

namespace Employee.Services.Employee.Employee
{
    public interface IEmployeeServices
    {
        IResponseDto SearchEmployees(EmployeeFilterDto filterDto);
        IResponseDto GetEmployeesDropdown();
        Task<IResponseDto> CreateEmployee(CreateEmployeeDto options);
        Task<IResponseDto> UpdateEmployee(int id, UpdateEmployeeDto options);
        Task<IResponseDto> RemoveEmployee(int id);
        Task<IResponseDto> Activate(int id);
        Task<IResponseDto> Deactivate(int id);
    }
}
