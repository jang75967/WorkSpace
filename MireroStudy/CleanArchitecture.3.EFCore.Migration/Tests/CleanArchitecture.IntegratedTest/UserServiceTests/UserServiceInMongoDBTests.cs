using Api.Users;
using Application.Mappers;
using CleanArchitecture.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Infrastructure.MongoDB;
using Xunit;
using EntityUser = Domain.Entities.User;

namespace CleanArchitecture.IntegratedTest.UserServiceTests;

public class UserServiceInMongoDBTests : TestBase<MongoDBFactory<Program, ApplicationDbContext>>
{
    private readonly GrpcChannel _channel;
    private readonly IMapper _mapper;
    public UserServiceInMongoDBTests(MongoDBFactory<Program, ApplicationDbContext> factory)
    {
        _channel = factory.Channel;
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_User_Count_Is_Five()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetUsersAsync(new GetUserRequest { });
        var result = _mapper.Map<IEnumerable<EntityUser>>(response.Users);
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
                second.Name.Should().Be("안성윤");
                second.Email.Should().Be("an@gmail.com");

            },
            third =>
            {
                third.Name.Should().Be("이건우");
                third.Email.Should().Be("lee@gmail.com");

            },
            fourth =>
            {
                fourth.Name.Should().Be("장동계");
                fourth.Email.Should().Be("jang@gmail.com");
            },
            fifth =>
            {
                fifth.Name.Should().Be("조범희");
                fifth.Email.Should().Be("jo@gmail.com");
            });
    }
}