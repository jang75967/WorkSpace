using FluentValidation;

namespace CleanArchitecture.Core.Application.Features.Users.Queries;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(query => query.UserId).GreaterThan(0);
    }
}
