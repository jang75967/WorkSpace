using Api.Activities;
using Application.Mappers;
using Application.Persistences;
using MediatR;

namespace CleanArchitecture.Core.Application.Features.Activities.Commands;

public record DeleteActivityCommand : IRequest<bool>
{
    public DeleteActivityRequest Request { get; }
    public DeleteActivityCommand(DeleteActivityRequest request)
        => Request = request;
}

public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand, bool>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMapper _mapper;
    public DeleteActivityCommandHandler(IActivityRepository activityRepository, IMapper mapper)
    {
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteActivityCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _activityRepository.DeleteAsync(request.Request.ActivityId, cancellationToken);
        return result;
    }
}
