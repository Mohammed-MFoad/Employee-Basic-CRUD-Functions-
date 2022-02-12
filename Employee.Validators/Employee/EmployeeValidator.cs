using Employee.DTO.Employee.Employee;
using FluentValidation;

namespace Signwave.Validators.Employee
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(e => e.FirstName)
             .Cascade(CascadeMode.Stop)
             .NotNull().WithMessage("First name should not be null")
             .NotEmpty().WithMessage("First name should not be empty")
             .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(e => e.LastName)
             .Cascade(CascadeMode.Stop)
             .NotNull().WithMessage("Last name should not be null")
             .NotEmpty().WithMessage("Last name should not be empty")
             .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(e => e.MiddleName)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(100).WithMessage("Middle name must not exceed 100 characters");

            RuleFor(e => e.Address)
             .NotNull().WithMessage("Address should not be null")
             .NotEmpty().WithMessage("Address should not be empty")
             .MaximumLength(1000).WithMessage("Address must not exceed 1000 characters");

            RuleFor(e => e.Email)
              .Cascade(CascadeMode.Stop)
              .EmailAddress().WithMessage("Email must be in 'xyz@xyz.com' format").When(e => !string.IsNullOrEmpty(e.Email));

            RuleFor(e => e.Phone)
             .MinimumLength(9).WithMessage("Phone must be greater than 9 characters").When(e => !string.IsNullOrEmpty(e.Phone))
             .MaximumLength(22).WithMessage("Phone must not exceed 15 characters").When(e => !string.IsNullOrEmpty(e.Phone));

        }
    }
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(e => e.FirstName)
              .Cascade(CascadeMode.Stop)
              .NotNull().WithMessage("First name should not be null")
              .NotEmpty().WithMessage("First name should not be empty")
              .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(e => e.LastName)
             .Cascade(CascadeMode.Stop)
             .NotNull().WithMessage("Last name should not be null")
             .NotEmpty().WithMessage("Last name should not be empty")
             .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(e => e.MiddleName)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(100).WithMessage("Middle name must not exceed 100 characters");

            RuleFor(e => e.Address)
             .NotNull().WithMessage("Address should not be null")
             .NotEmpty().WithMessage("Address should not be empty")
             .MaximumLength(1000).WithMessage("Address must not exceed 1000 characters");

            RuleFor(e => e.Email)
             .Cascade(CascadeMode.Stop)
             .EmailAddress().WithMessage("Email must be in 'xyz@xyz.com' format").When(e => !string.IsNullOrEmpty(e.Email));

            RuleFor(e => e.Phone)
             .MinimumLength(9).WithMessage("Phone must be greater than 9 characters").When(e => !string.IsNullOrEmpty(e.Phone))
             .MaximumLength(22).WithMessage("Phone must not exceed 15 characters").When(e => !string.IsNullOrEmpty(e.Phone));
        }
    }
}
