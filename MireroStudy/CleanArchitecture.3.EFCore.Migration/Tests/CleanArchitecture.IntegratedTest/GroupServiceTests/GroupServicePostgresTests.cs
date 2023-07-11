using Api.Users;
using Application.Mappers;
using CleanArchitecture.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Infrastructure.EFCore;
using Xunit;
using EntityGroup = Domain.Entities.Group;
namespace CleanArchitecture.IntegratedTest.GroupServiceTests;

public class GroupServicePostgresTests : TestBase<PostgresFactory<Program, ApplicationDbContext>>
{
    private readonly GrpcChannel _channel;
    private readonly IMapper _mapper;
    public GroupServicePostgresTests(PostgresFactory<Program, ApplicationDbContext> factory)
    {
        _channel = factory.Channel;
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Groups()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetGroupsAsync(new GetGroupRequest { });
        var result = _mapper.Map<IEnumerable<EntityGroup>>(response.Groups);
        // Assert

        result.Should().HaveCount(2);
        result.Should().SatisfyRespectively(
            first =>
            {
                first.Name.Should().Be("농구 동아리");
            },
            second =>
            {
                second.Name.Should().Be("축구 동아리");

            });
    }
}