using Api.Users;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Xunit;

namespace CleanArchitecture.UnitTest;

public class UserServiceTests : TestBase
{
    private readonly GrpcChannel _channel;
    public UserServiceTests(TestFactory<Program> factory) : base(factory)
    {
        _channel = factory.Channel;
    }

    [Fact]
    public async Task Should_Be_User_Count_Is_Five()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetUsersAsync(new GetUserRequest { });

        // Assert
        response.Users.Count.Should().Be(4);
    }
}
