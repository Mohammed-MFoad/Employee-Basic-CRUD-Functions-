using Autofac;
using Employee.DTO.Common;
using Employee.Services.Employee.Employee;

namespace Employee.Services
{
    public class AutoFacConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<EmployeeServices>().As<IEmployeeServices>().InstancePerDependency();
            builder.RegisterType<ResponseDto>().As<IResponseDto>().InstancePerLifetimeScope();
        }   
    }
}
