using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Groups.Queries;
using CleanArchitecture.UnitTest.EFCore.Groups.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Groups.Queries;

public class GetAllGroupsQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;

    public GetAllGroupsQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_All_Groups()
    {
        // Arrange
        var groupRepository = new GroupRepository(new MockDbContext().Get().Object);
        var handler = new GetAllGroupsQueryHandler(groupRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetAllGroupsQuery());

        // Assert
        result.Should().HaveCount(2);
        result.Should().SatisfyRespectively(
            first =>
            {
                first.Name.Should().Be("축구 동아리");
            },
            second =>
            {
                second.Name.Should().Be("농구 동아리");
            });
    }
}
