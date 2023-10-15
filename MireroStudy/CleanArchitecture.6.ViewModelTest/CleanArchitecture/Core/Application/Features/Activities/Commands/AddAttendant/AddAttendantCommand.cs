using Api.Activities;
using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoActivity = Api.Activities.Activity;
using EntityAttendant = Domain.Entities.ActivityAggregate.Attendant;
namespace CleanArchitecture.Core.Application.Features.Activities.Commands;


public record AddAttendantCommand : IRequest<DtoActivity>
{
    public AddAttendantRequest Request { get; }
    public AddAttendantCommand(AddAttendantRequest request)
        => Request = request;
}

public class AddAttendantCommandHandler : IRequestHandler<AddAttendantCommand, DtoActivity>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public AddAttendantCommandHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<DtoActivity> Handle(AddAttendantCommand request, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetAsync(request.Request.ActivityId, cancellationToken);

        var Attendant = _mapper.Map<EntityAttendant>(request.Request.Attendant);
        activity.AddAttendant(Attendant);

        var entity = await _activityRepository.UpdateAsync(activity, cancellationToken);
        var dto = _mapper.Map<DtoActivity>(entity);

        return dto;
    }
}
