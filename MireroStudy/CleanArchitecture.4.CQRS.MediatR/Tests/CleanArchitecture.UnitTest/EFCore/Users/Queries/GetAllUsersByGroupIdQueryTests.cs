using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Users.Queries;
using CleanArchitecture.UnitTest.EFCore.Users.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Users.Queries;

public class GetAllUsersByGroupIdQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;

    public GetAllUsersByGroupIdQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_Members_By_GroupId()
    {
        // Arrange
        var userRepository = new UserRepository(new MockDbContext().Get().Object);
        var handler = new GetAllUsersByGroupIdQueryHandler(userRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetAllUsersByGroupIdQuery(1));

        // Assert
        result.Should().HaveCount(3);
        result.Should().SatisfyRespectively(
            first =>
            {
                first.Name.Should().Be("박영석");
                first.Email.Should().Be("bak@gmail.com");
            },
            second =>
            {
                second.Name.Should().Be("이건우");
                second.Email.Should().Be("lee@gmail.com");
            },
            third =>
            {
                third.Name.Should().Be("장동계");
                third.Email.Should().Be("jang@gmail.com");
            });
    }
}
