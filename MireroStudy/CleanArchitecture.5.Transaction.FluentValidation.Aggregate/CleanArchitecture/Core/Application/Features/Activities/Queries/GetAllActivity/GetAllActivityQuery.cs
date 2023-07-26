using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoActivity = Api.Activities.Activity;
namespace CleanArchitecture.Core.Application.Features.Activities.Queries;

public record GetAllActivityQuery : IRequest<IEnumerable<DtoActivity>>;

public class GetAllActivityQueryHandler : IRequestHandler<GetAllActivityQuery, IEnumerable<DtoActivity>>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public GetAllActivityQueryHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DtoActivity>> Handle(GetAllActivityQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _activityRepository.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<DtoActivity>>(entities);

        return dtos;
    }
}
