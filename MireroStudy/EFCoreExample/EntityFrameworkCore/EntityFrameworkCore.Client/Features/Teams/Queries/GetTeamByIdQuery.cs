using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Queries;

public record GetTeamByIdQuery(int id) : IRequest<Team>;
