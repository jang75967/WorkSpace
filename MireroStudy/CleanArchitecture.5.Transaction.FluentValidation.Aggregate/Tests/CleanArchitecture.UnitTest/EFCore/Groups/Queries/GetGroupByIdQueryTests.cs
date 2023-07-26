using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Groups.Queries;
using CleanArchitecture.UnitTest.EFCore.Groups.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Groups.Queries;

public class GetGroupByIdQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;

    public GetGroupByIdQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_Group_By_GroupId()
    {
        // Arrange
        var groupRepository = new GroupRepository(new MockDbContext().Get().Object);
        var handler = new GetGroupByIdQueryHandler(groupRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetGroupByIdQuery(1));

        // Assert
        result.Id.Should().Be(1);
        result.Name.Should().Be("축구 동아리");
    }
}
