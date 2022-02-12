using Employee.Data.DataContext;
using Employee.DTO.Common;
using Employee.DTO.Employee.Employee;
using Mapster;
using System.Linq.Dynamic.Core;
namespace Employee.Services.Employee.Employee
{
    public class EmployeeServices : IEmployeeServices
    {

        private readonly AppDbContext _appDbContext;
        private readonly IResponseDto _response;

        public EmployeeServices(AppDbContext appDbContext,
            IResponseDto response)
        {
            _response = response;
            _appDbContext = appDbContext;
        }

        public IResponseDto SearchEmployees(EmployeeFilterDto filterDto)
        {
            try
            {
                var employees = _appDbContext.Employees.Where(x => !x.IsDeleted);

                if (filterDto != null)
                {
                    if (!string.IsNullOrEmpty(filterDto.Name))
                    {
                        employees = employees.Where(x => 
                        (x.FirstName != null && x.FirstName.Trim().ToLower().Contains(filterDto.Name.Trim().ToLower())) ||
                        (x.LastName != null && x.LastName.Trim().ToLower().Contains(filterDto.Name.Trim().ToLower())) ||
                        (x.MiddleName != null && x.MiddleName.Trim().ToLower().Contains(filterDto.Name.Trim().ToLower())));
                    }
                    if (!string.IsNullOrEmpty(filterDto.Address))
                    {
                        employees = employees.Where(x =>
                        (x.Address != null && x.Address.Trim().ToLower().Contains(filterDto.Address.Trim().ToLower())));
                    }
                    if (!string.IsNullOrEmpty(filterDto.Phone))
                    {
                        employees = employees.Where(x =>
                        (x.Phone != null && x.Phone.Trim().ToLower().Contains(filterDto.Phone.Trim().ToLower())));
                    }
                    if (!string.IsNullOrEmpty(filterDto.Email))
                    {
                        employees = employees.Where(x =>
                        (x.Email != null && x.Email.Trim().ToLower().Contains(filterDto.Email.Trim().ToLower())));
                    }
                }

                //Check Sort Property
                if (!string.IsNullOrEmpty(filterDto?.SortProperty))
                {
                    employees = employees.OrderBy(
                        string.Format("{0} {1}", filterDto.SortProperty, filterDto.IsAscending ? "ASC" : "DESC"));
                }
                else
                {
                    employees = employees.OrderByDescending(x => x.Id);
                }

                // Pagination
                var total = employees.Count();
                if (filterDto != null && filterDto.PageIndex.HasValue && filterDto.PageSize.HasValue)
                {
                    employees = employees.Skip((filterDto.PageIndex.Value - 1) * filterDto.PageSize.Value).Take(filterDto.PageSize.Value);
                }
             

                var datalist = employees.Adapt<List<EmployeeDto>>();

                _response.IsPassed = true;
                _response.Data = new
                {
                    List = datalist,
                    Total = total,
                };
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }

            return _response;
        }
        public IResponseDto GetEmployeesDropdown()
        {
            try
            {
                var employees = _appDbContext.Employees.Where(x => !x.IsDeleted && x.IsActive)
                            .Select(x => new 
                            {
                                Id = x.Id,
                                Name = (x.FirstName + " " + x.LastName)
                            }).OrderBy(x => x.Id).ToList();


                _response.IsPassed = true;
                _response.Data = employees;
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }

            return _response;
        }
        public async Task<IResponseDto> CreateEmployee(CreateEmployeeDto options)
        {
            try
            {
                var employee = options.Adapt<Data.DbModels.EmployeeSchema.Employee>();

                // Validate
                var validationResult = ValidateEmployee(employee);
                if (!validationResult.IsPassed)
                {
                    return validationResult;
                }

                employee.CreatedOn = DateTime.Now;
                employee.IsActive = true;

                // save to the database
                _appDbContext.Employees.Attach(employee);
                var save = await _appDbContext.SaveChangesAsync();
                if (save == 0)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("There  are  no changes to be saved");
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "Employee is created successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }

            return _response;
        }
        public async Task<IResponseDto> UpdateEmployee(int id, UpdateEmployeeDto options)
        {
            try
            {
                var employee = await _appDbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("The specified employee does not exist.");
                    return _response;
                }
                else if (employee.IsDeleted)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"The specified employee '{employee.FirstName} {employee.LastName}' is deleted.");
                    return _response;
                }
                else
                {
                    //Set Data
                    employee.FirstName = options.FirstName;
                    employee.LastName = options.LastName;
                    employee.MiddleName = options.MiddleName;
                    employee.Address = options.Address;
                    employee.Email = options.Email;
                    employee.Phone = options.Phone;
                    employee.UpdatedOn = DateTime.Now;
                }

                // Validate
                var validationResult = ValidateEmployee(employee, id);
                if (!validationResult.IsPassed)
                {
                    return validationResult;
                }

                // save to the database
                _appDbContext.Employees.Attach(employee);
                var save = await _appDbContext.SaveChangesAsync();
                if (save == 0)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("There  are  no changes to be saved");
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "Employee is updated successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }
            return _response;
        }
        public async Task<IResponseDto> RemoveEmployee(int id)
        {
            try
            {
                var employee = await _appDbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("The specified employee does not exist.");
                    return _response;
                }
                else if (employee.IsDeleted)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"The specified employee '{employee.FirstName} {employee.LastName}' is already deleted.");
                    return _response;
                }

                employee.IsDeleted = true;
                employee.UpdatedOn = DateTime.Now;

                // save to the database
                _appDbContext.Employees.Attach(employee);
                var save = await _appDbContext.SaveChangesAsync();
                if (save == 0)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("There  are  no changes to be saved");
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "Employee is removed successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }

            return _response;
        }
        public async Task<IResponseDto> Activate(int id)
        {
            try
            {
                var employee = await _appDbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("The specified employee does not exist.");
                    return _response;
                }
                else if (employee.IsDeleted)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"The specified employee '{employee.FirstName} {employee.LastName}' is deleted.");
                    return _response;
                }
                else if (employee.IsActive)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"The specified employee '{employee.FirstName} {employee.LastName}' is already activated.");
                    return _response;
                }

                employee.IsActive = true;
                employee.UpdatedOn = DateTime.Now;

                _appDbContext.Employees.Attach(employee);
                var save = await _appDbContext.SaveChangesAsync();
                if (save == 0)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("There  are  no changes to be saved");
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "employee is updated successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }

            return _response;
        }
        public async Task<IResponseDto> Deactivate(int id)
        {
            try
            {
                var employee = await _appDbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("The specified employee does not exist.");
                    return _response;
                }
                else if (employee.IsDeleted)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"The specified employee '{employee.FirstName} {employee.LastName}' is deleted.");
                    return _response;
                }
                else if (!employee.IsActive)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"The specified employee '{employee.FirstName} {employee.LastName}' is already deactivated.");
                    return _response;
                }

                employee.IsActive = false;
                employee.UpdatedOn = DateTime.Now;

                _appDbContext.Employees.Attach(employee);
                var save = await _appDbContext.SaveChangesAsync();
                if (save == 0)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("There  are  no changes to be saved");
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "employee is updated successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }

            return _response;
        }
        // Helper Methods
        private IResponseDto ValidateEmployee(Data.DbModels.EmployeeSchema.Employee employee, int id = 0)
        {
            try
            {
                if (_appDbContext.Employees.Any(x => x.Id != id &&
                            !x.IsDeleted && x.FirstName != null && x.LastName != null && employee.FirstName != null && employee.LastName != null &&
                            (x.FirstName.ToLower().Trim() + x.LastName.ToLower().Trim()) == (employee.FirstName.ToLower().Trim() +  employee.LastName.ToLower().Trim())))
                {
                    _response.Errors.Add($"Employee '{employee.FirstName} {employee.LastName}' already exists, please try a new one.'");
                }

                _response.IsPassed = true;
            }
            catch (Exception ex)
            {
                _response.Data = null!;
                _response.IsPassed = false;
                _response.Errors.Add($"Error: {ex.Message}");
            }

            if (_response.Errors.Count > 0)
            {
                _response.Errors = _response.Errors.Distinct().ToList();
                _response.IsPassed = false;
                _response.Data = null!;
                return _response;
            }

            return _response;
        }
    }
}
