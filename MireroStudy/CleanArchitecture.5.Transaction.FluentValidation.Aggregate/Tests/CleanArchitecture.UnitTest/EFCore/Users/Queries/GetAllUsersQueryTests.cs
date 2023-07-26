using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Users.Queries;
using CleanArchitecture.UnitTest.EFCore.Users.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Users.Queries;

public class GetAllUsersQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;

    public GetAllUsersQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_Members_By_GroupId()
    {
        // Arrange
        var userRepository = new UserRepository(new MockDbContext().Get().Object);
        var handler = new GetAllUsersQueryHandler(userRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetAllUsersQuery());

        // Assert
        result.Should().HaveCount(5);
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
                third.Name.Should().Be("조범희");
                third.Email.Should().Be("jo@gmail.com");
            },
            fourth =>
            {
                fourth.Name.Should().Be("안성윤");
                fourth.Email.Should().Be("an@gmail.com");
            },
            fifth =>
            {
                fifth.Name.Should().Be("장동계");
                fifth.Email.Should().Be("jang@gmail.com");
            });
    }
}