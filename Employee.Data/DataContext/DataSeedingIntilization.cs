using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Employee.Data.DataContext
{
    public class DataSeedingIntilization
    {
        private static AppDbContext _appDbContext = null!;

        public static void Seed(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _appDbContext.Database.Migrate();

            SeedEmployees();

            _appDbContext.SaveChanges();
        }

        private static void SeedEmployees()
        {
            var employeeCount = _appDbContext.Employees.Count();
            var employees = new List<DbModels.EmployeeSchema.Employee>();
            if (employeeCount == 0)
            {
                employees = Enumerable.Repeat(0, 5)
                            .Select(_ => new Faker<DbModels.EmployeeSchema.Employee>()
                            .CustomInstantiator(f => new DbModels.EmployeeSchema.Employee()
                            {
                                FirstName = f.Name.FirstName(),
                                LastName = f.Name.LastName(),
                                MiddleName = f.Name.FirstName(),
                                Email = f.Internet.Email(),
                                Address = f.Address.FullAddress(),
                                Phone = f.Phone.PhoneNumber(),
                                IsActive = true,
                                CreatedOn = DateTime.Now,
                            })
                            .Generate()).ToList();
            }
            _appDbContext.Employees.AddRange(employees);
        }
    }
}
