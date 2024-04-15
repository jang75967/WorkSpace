using EntityFrameworkCore.Domain;
using MediatR;

namespace EntityFrameworkCore.Client.Features.Coaches.Queries;

public record GetCoachByIdQuery(int id) : IRequest<Coach>;