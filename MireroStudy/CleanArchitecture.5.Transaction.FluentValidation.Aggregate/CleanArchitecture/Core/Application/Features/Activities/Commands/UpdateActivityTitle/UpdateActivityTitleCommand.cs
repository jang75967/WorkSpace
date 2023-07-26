using Api.Activities;
using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoActivity = Api.Activities.Activity;
using EntityActivity = Domain.Entities.ActivityAggregate.Activity;
namespace CleanArchitecture.Core.Application.Features.Activities.Commands;

public class UpdateActivityTitleCommand : IRequest<DtoActivity>
{
    public UpdateActivityTitleRequest Request { get; }
    public UpdateActivityTitleCommand(UpdateActivityTitleRequest request)
        => Request = request;
}

public class UpdateActivityTitleCommandHandler : IRequestHandler<UpdateActivityTitleCommand, DtoActivity>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public UpdateActivityTitleCommandHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<DtoActivity> Handle(UpdateActivityTitleCommand request, CancellationToken cancellationToken = default)
    {
        var entity = await _activityRepository.GetAsync(request.Request.ActivityId, cancellationToken);
        entity.Title = request.Request.Title;
        var result = await _activityRepository.UpdateAsync(entity, cancellationToken);
        var dto = _mapper.Map<DtoActivity>(result);
        return dto;
    }
}