using Api.Activities;
using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Activities.Commands;
using CleanArchitecture.UnitTest.EFCore.Activities.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Moq;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Activities.Commands;

public class DeleteActivityCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public DeleteActivityCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Remove_Activity()
    {
        var request = new DeleteActivityRequest()
        {
            ActivityId = 1
        };
        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var activityRepository = new ActivityRepository(mockDbContext.Object);
        var handler = new DeleteActivityCommandHandler(activityRepository, _mapper);

        // Act
        var result = await handler.Handle(new DeleteActivityCommand(request));

        // Assert
        result.Should().BeTrue();

        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}