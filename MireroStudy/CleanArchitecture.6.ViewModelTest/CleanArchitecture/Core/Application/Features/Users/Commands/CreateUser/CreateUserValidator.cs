using FluentValidation;

namespace CleanArchitecture.Core.Application.Features.Users.Commands;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(query => query.User.Id).Equal(0);
        RuleFor(query => query.User.Name)
            .NotNull().NotEmpty()
            .WithMessage("User name is required")
            .Length(1, 30)
            .WithMessage("User name must be between 1 and 30 characters.");

        RuleFor(query => query.User.Email)
            .NotNull().NotEmpty()
            .WithMessage("User email is required.")
            .MaximumLength(30)
            .WithMessage("User email should have 30 chars at most.")
            .EmailAddress()
            .WithMessage("A valid email is required.");

        RuleFor(query => query.User.Password)
            .NotNull().NotEmpty()
            .WithMessage("User password is required.")
            .Length(1, 15)
            .WithMessage("User password must be between 1 and 15 characters.");
    }
}
