using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Commands;

public record AddTeamCommand(Team Team) : IRequest;
