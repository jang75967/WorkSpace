using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Teams.Queries;

public record GetTeamsQuery : IRequest<IEnumerable<Team>>;
