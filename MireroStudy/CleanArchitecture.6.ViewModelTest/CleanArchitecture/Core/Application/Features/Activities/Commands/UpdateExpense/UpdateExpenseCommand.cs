using Application.Persistences;
using MediatR;
using DtoExpense = Api.Activities.Expense;
using DtoActivity = Api.Activities.Activity;
using EntityExpense = Domain.Entities.ActivityAggregate.Expense;
using Application.Mappers;
using Api.Activities;

namespace CleanArchitecture.Core.Application.Features.Activities.Commands;


public class UpdateExpenseCommand : IRequest<DtoActivity>
{

    public UpdateExpenseRequest Request { get; }
    public UpdateExpenseCommand(UpdateExpenseRequest request)
        => Request = request;
}

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, DtoActivity>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public UpdateExpenseCommandHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<DtoActivity> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetAsync(request.Request.ActivityId, cancellationToken);

        var expense = _mapper.Map<EntityExpense>(request.Request.Expense);
        activity.UpdateExpense(expense);

        var entity = await _activityRepository.UpdateAsync(activity, cancellationToken);
        var dto = _mapper.Map<DtoActivity>(entity);

        return dto;
    }
}