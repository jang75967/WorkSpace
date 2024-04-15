using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Commands;

public record AddCoachCommand(Coach Coach) : IRequest;
