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

public class UpdateActivityTitleCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public UpdateActivityTitleCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Create_Activity()
    {
        var request = new UpdateActivityTitleRequest()
        {
            ActivityId = 1,
            Title = "올림픽참가",
        };
        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var activityRepository = new ActivityRepository(mockDbContext.Object);
        var handler = new UpdateActivityTitleCommandHandler(activityRepository, _mapper);

        // Act
        var result = await handler.Handle(new UpdateActivityTitleCommand(request));

        // Assert
        result.Title.Should().Be("올림픽참가");

        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}