using FluentValidation;

namespace CleanArchitecture.Core.Application.Features.Users.Queries;

public class GetAllUsersByGroupIdValidator : AbstractValidator<GetAllUsersByGroupIdQuery>
{
    public GetAllUsersByGroupIdValidator()
    {
        RuleFor(query => query.GroupId).GreaterThan(0);
    }
}
