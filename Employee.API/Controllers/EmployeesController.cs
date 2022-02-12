using Employee.DTO.Common;
using Employee.DTO.Employee.Employee;
using Employee.Services.Employee.Employee;
using Microsoft.AspNetCore.Mvc;
using Signwave.Validators.Employee;

namespace InternalBond.API.Controllers
{

    public class EmployeesController
    {
        private readonly IEmployeeServices _employeeService;
        private IResponseDto _response;

        public EmployeesController(
           IEmployeeServices employeeService,
           IResponseDto response) 
        {
            _employeeService = employeeService;
            _response = response;
        }


        [Route("api/employees")]
        [HttpGet]
        public IResponseDto SearchEmployeees([FromQuery] EmployeeFilterDto filterDto)
        {
            _response = _employeeService.SearchEmployees(filterDto);
            return _response;
        }

     
        [Route("api/employees/list")]
        [HttpGet]
        public IResponseDto GetEmployeeesDropdown()
        {
            _response = _employeeService.GetEmployeesDropdown();
            return _response;
        }


        [Route("api/employees")]
        [HttpPost]
        public async Task<IResponseDto> CreateEmployee([FromBody] CreateEmployeeDto options)
        {
            var validationResult = await (new CreateEmployeeValidator()).ValidateAsync(options);
            if (!validationResult.IsValid)
            {
                _response.IsPassed = false;
                _response.Message = string.Join(",\n\r", validationResult.Errors.Select(e => e.ErrorMessage));
                _response.Data = null!;
                return _response;
            }

            _response = await _employeeService.CreateEmployee(options);
            return _response;
        }


        [Route("api/employees/{id}")]
        [HttpPut]
        public async Task<IResponseDto> UpdateEmployee([FromRoute] int id, [FromBody] UpdateEmployeeDto options)
        {
            var validationResult = await (new UpdateEmployeeValidator()).ValidateAsync(options);
            if (!validationResult.IsValid)
            {
                _response.IsPassed = false;
                _response.Message = string.Join(",\n\r", validationResult.Errors.Select(e => e.ErrorMessage));
                _response.Data = null!;
                return _response;
            }

            _response = await _employeeService.UpdateEmployee(id, options);
            return _response;
        }


        [Route("api/employees/{id}")]
        [HttpDelete]
        public async Task<IResponseDto> RemoveEmployee([FromRoute] int id)
        {
            _response = await _employeeService.RemoveEmployee(id);
            return _response;
        }


        [Route("api/employees/{id}/activate")]
        [HttpPost]
        public async Task<IResponseDto> Activate([FromRoute] int id)
        {
            _response = await _employeeService.Activate(id);
            return _response;
        }

        [Route("api/employees/{id}/deactivate")]
        [HttpPost]
        public async Task<IResponseDto> Deactivate([FromRoute] int id)
        {
            _response = await _employeeService.Deactivate(id);
            return _response;
        }
    }
}
