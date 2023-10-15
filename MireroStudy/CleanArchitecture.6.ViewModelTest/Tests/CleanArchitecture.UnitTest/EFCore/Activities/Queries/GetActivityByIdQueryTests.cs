using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Activities.Queries;
using CleanArchitecture.UnitTest.EFCore.Activities.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Activities.Queries;


public class GetActivityByIdQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;

    public GetActivityByIdQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_Activity_By_Id()
    {
        // Arrange
        var activityRepository = new ActivityRepository(new MockDbContext().Get().Object);
        var handler = new GetActivityByIdQueryHandler(activityRepository, _mapper);
        var id = 1;
        // Act
        var result = await handler.Handle(new GetActivityByIdQuery(id));

        // Assert
        result.Id.Should().Be(1);
        result.GroupId.Should().Be(1);
        result.Title.Should().Be("체육대회");
        result.Attendees.Count.Should().Be(2);
        result.Expenses.Count.Should().Be(2);
    }
}

