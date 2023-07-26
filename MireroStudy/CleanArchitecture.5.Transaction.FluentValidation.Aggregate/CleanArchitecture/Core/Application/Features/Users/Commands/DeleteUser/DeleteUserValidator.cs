using CleanArchitecture.Core.Application.Features.Users.Queries;
using FluentValidation;

namespace CleanArchitecture.Core.Application.Features.Users.Commands;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        
    }
}
