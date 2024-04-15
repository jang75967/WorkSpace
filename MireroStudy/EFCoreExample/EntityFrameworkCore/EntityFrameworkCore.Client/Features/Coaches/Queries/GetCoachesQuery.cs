using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Queries;

public record GetCoachesQuery : IRequest<IEnumerable<Coach>>;
