using Api.Activities;
using Application.Mappers;
using Application.Persistences;
using MediatR;
using DtoActivity = Api.Activities.Activity;
using EntityExpense = Domain.Entities.ActivityAggregate.Expense;

namespace CleanArchitecture.Core.Application.Features.Activities.Commands;

public record RemoveExpenseCommand : IRequest<DtoActivity>
{
    public RemoveExpenseRequest Request { get; }
    public RemoveExpenseCommand(RemoveExpenseRequest request)
        => Request = request;
}

public class RemoveExpenseCommandHandler : IRequestHandler<RemoveExpenseCommand, DtoActivity>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public RemoveExpenseCommandHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<DtoActivity> Handle(RemoveExpenseCommand request, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetAsync(request.Request.ActivityId, cancellationToken);

        var expense = _mapper.Map<EntityExpense>(request.Request.Expense);
        activity.RemoveExpense(expense);

        var entity = await _activityRepository.UpdateAsync(activity, cancellationToken);
        var dto = _mapper.Map<DtoActivity>(entity);

        return dto;
    }
}