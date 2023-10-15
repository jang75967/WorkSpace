using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Activities.Queries;
using CleanArchitecture.UnitTest.EFCore.Activities.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Activities.Queries;

public class GetAllActivityQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;

    public GetAllActivityQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_All_Activities()
    {
        // Arrange
        var activityRepository = new ActivityRepository(new MockDbContext().Get().Object);
        var handler = new GetAllActivityQueryHandler(activityRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetAllActivityQuery());

        // Assert
        result.Should().HaveCount(2);
        result.Should().SatisfyRespectively(
            first =>
            {
                first.Title.Should().Be("체육대회");
            },
            second =>
            {
                second.Title.Should().Be("체육대회");
            });
    }
}
