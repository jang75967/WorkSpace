using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoActivity = Api.Activities.Activity;
using DtoCreateActivityRequest = Api.Activities.CreateActivityRequest;
using EntityActivity = Domain.Entities.ActivityAggregate.Activity;
using EntityAttendant = Domain.Entities.ActivityAggregate.Attendant;
using EntityExpense = Domain.Entities.ActivityAggregate.Expense;

namespace CleanArchitecture.Core.Application.Features.Activities.Commands;

public record CreateActivityCommand : IRequest<DtoActivity>
{
    public DtoCreateActivityRequest Request { get; }
    public CreateActivityCommand(DtoCreateActivityRequest request)
        => Request = request;
}

public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, DtoActivity>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public CreateActivityCommandHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<DtoActivity> Handle(CreateActivityCommand request, CancellationToken cancellationToken = default)
    {
        var activity = EntityActivity.Create(request.Request.Title, request.Request.GroupId);

        var attendees = _mapper.Map<IEnumerable<EntityAttendant>>(request.Request.Attendees);
        foreach(var attendant in attendees)
            activity.AddAttendant(attendant);

        var expenses = _mapper.Map<IEnumerable<EntityExpense>>(request.Request.Expenses);
        foreach(var expense in expenses)
            activity.AddExpense(expense);

        var result = await _activityRepository.CreateAsync(activity, cancellationToken);
        var dto = _mapper.Map<DtoActivity>(result);
        return dto;
    }
}