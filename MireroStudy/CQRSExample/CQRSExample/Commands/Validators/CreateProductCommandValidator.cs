using CQRSExample.Commands;
using FluentValidation;

namespace CQRSExample.Commands.Validators;

public class CreateProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(cmd => cmd.Product).NotNull();
        RuleFor(cmd => cmd.Product.Id).NotEmpty();
        RuleFor(cmd => cmd.Product.Name).NotEmpty();
    }
}
