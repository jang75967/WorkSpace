using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Groups.Queries;
using CleanArchitecture.UnitTest.EFCore.Groups.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Groups.Queries;

public class GetGroupsByUserIdQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;

    public GetGroupsByUserIdQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_Group_By_UserId()
    {
        // Arrange
        var groupRepository = new GroupRepository(new MockDbContext().Get().Object);
        var handler = new GetGroupsByUserIdQueryHandler(groupRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetGroupsByUserIdQuery(2));

        // Assert
        result.Should().HaveCount(1);
        result.Should().SatisfyRespectively(
            first =>
            {
                first.Name.Should().Be("축구 동아리");
            });
    }
}
