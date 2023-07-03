using Api.Users;
using Application.Mappers;
using CleanArchitecture.UnitTest.Factories;
using FluentAssertions;
using Grpc.Net.Client;
using Xunit;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;

namespace CleanArchitecture.UnitTest;

public class UserServiceTests
{
    private readonly GrpcChannel _channel;
    private readonly IMapper _mapper;
    public UserServiceTests(TestFactory<Program> factory)
    {
        _channel = factory.Channel;
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Get_DtoType_Users()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetUsersAsync(new GetUserRequest { });

        // Assert
        var user = response.Users.First();

        user.GetType().Should().Be(typeof(DtoUser));
        response.Users.Should().HaveCount(1);
    }

    [Fact]
    public async Task Should_Be_Get_EntityType_Users()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetUsersAsync(new GetUserRequest { });
        var entities = _mapper.Map<IEnumerable<EntityUser>>(response.Users);

        // Assert
        entities.Should().HaveCount(1);

        var user = entities.First();
        user.GetType().Should().Be(typeof(EntityUser));
    }
}
