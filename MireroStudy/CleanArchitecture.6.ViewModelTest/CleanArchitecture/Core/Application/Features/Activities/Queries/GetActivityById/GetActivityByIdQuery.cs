using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoActivity = Api.Activities.Activity;
namespace CleanArchitecture.Core.Application.Features.Activities.Queries;

public record GetActivityByIdQuery : IRequest<DtoActivity>
{
    public long ActivityId { get; }
    public GetActivityByIdQuery(long activityId)
        => ActivityId = activityId;
}

public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, DtoActivity>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public GetActivityByIdQueryHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<DtoActivity> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _activityRepository.GetAsync(request.ActivityId, cancellationToken);
        var dto = _mapper.Map<DtoActivity>(entity);
        return dto;
    }
}