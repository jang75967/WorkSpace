using Api.Activities;
using Azure.Core;
using CleanArchitecture.Core.Application.Features.Activities.Commands;
using Grpc.Core;
using MediatR;

namespace CleanArchitecture.Controllers;

public class ActivityController : ActivitiesGrpc.ActivitiesGrpcBase
{
    private readonly ILogger<ActivityController> _logger;
    private readonly IMediator _mediator;

    public ActivityController(
        ILogger<ActivityController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<CreateActivityReply> CreateActivity(CreateActivityRequest request, ServerCallContext context)
           => new CreateActivityReply() { Activity = await _mediator.Send(new CreateActivityCommand(request)) };

    public override async Task<DeleteActivityReply> DeleteActivity(DeleteActivityRequest request, ServerCallContext context)
           => new DeleteActivityReply() { Result = await _mediator.Send(new DeleteActivityCommand(request)) };

    public override async Task<AddExpenseReply> AddExpense(AddExpenseRequest request, ServerCallContext context)
        => new AddExpenseReply() { Activity = await _mediator.Send(new AddExpenseCommand(request)) };

    public override async Task<RemoveExpenseReply> RemoveExpense(RemoveExpenseRequest request, ServerCallContext context)
        => new RemoveExpenseReply() { Activity = await _mediator.Send(new RemoveExpenseCommand(request)) };

    public override async Task<UpdateExpenseReply> UpdateExpense(UpdateExpenseRequest request, ServerCallContext context)
        => new UpdateExpenseReply() { Activity = await _mediator.Send(new UpdateExpenseCommand(request)) };

    public override async Task<AddAttendantReply> AddAttendant(AddAttendantRequest request, ServerCallContext context)
        => new AddAttendantReply() { Activity = await _mediator.Send(new AddAttendantCommand(request)) };

    public override async Task<RemoveAttendantReply> RemoveAttendant(RemoveAttendantRequest request, ServerCallContext context)
        => new RemoveAttendantReply() { Activity = await _mediator.Send(new RemoveAttendantCommand(request)) };
}
