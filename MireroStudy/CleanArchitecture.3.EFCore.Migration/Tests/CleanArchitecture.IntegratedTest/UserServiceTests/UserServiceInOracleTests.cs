using Api.Users;
using CleanArchitecture.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Infrastructure.EFCore;
using Xunit;

namespace CleanArchitecture.IntegratedTest.UserServiceTests;

public class UserServiceInOracleTests : TestBase<OracleFactory<Program, ApplicationDbContext>>
{
    private readonly GrpcChannel _channel;
    public UserServiceInOracleTests(OracleFactory<Program, ApplicationDbContext> factory)
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
        response.Should().NotBeNull();
    }
}