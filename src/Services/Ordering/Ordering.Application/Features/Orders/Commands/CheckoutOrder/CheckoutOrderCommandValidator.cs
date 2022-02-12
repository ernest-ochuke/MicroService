using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage("{UserName} is required.")
                        .NotNull()
                        .MaximumLength(50).WithMessage("{UserName} must nit exceed 50 characters");

            RuleFor(p=>p.EmailAddress)
            .NotNull().WithMessage("{EmailAddress} is required.");

            RuleFor(p=>p.TotalPrice)
            .NotNull().WithMessage("{EmailAddress} is required.")
            .GreaterThan(0).WithMessage("{TotalPrice} should be greaterthan zero");
        }
    }
}