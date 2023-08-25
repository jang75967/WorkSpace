using Api.Groups;
using Application.Mappers;
using CleanArchitecture.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Infrastructure.EFCore;
using Xunit;
using EntityGroup = Domain.Entities.Group;
namespace CleanArchitecture.IntegratedTest;

[TestCaseOrderer(ordererTypeName: "Common.PriorityOrderer", ordererAssemblyName: "Common")]
public class GroupControllerTests : TestBase<PostgresFactory<Program, ApplicationDbContext>>
{
    private readonly GrpcChannel _channel;
    private readonly IMapper _mapper;
    public GroupControllerTests(PostgresFactory<Program, ApplicationDbContext> factory)
    {
        _channel = factory.Channel;
        _mapper = factory.Mapper;
    }

    [Fact, TestPriority(0)]
    public async Task Should_Get_All_Groups()
    {
        // Arrange
        var client = new GroupsGrpc.GroupsGrpcClient(_channel);

        // Act
        var response = await client.GetAllGroupsAsync(new GetAllGroupsRequest { });
        var result = _mapper.Map<IEnumerable<EntityGroup>>(response.Groups);

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

    [Fact, TestPriority(1)]
    public async Task Should_Get_Group_By_GroupId()
    {
        // Arrange
        var client = new GroupsGrpc.GroupsGrpcClient(_channel);

        // Act
        var response = await client.GetGroupByIdAsync(new GetGroupByIdRequest { Id = 1});
        var result = _mapper.Map<EntityGroup>(response.Group);

        // Assert
        result.Id.Should().Be(1);
        result.Name.Should().Be("축구 동아리");
    }

    [Fact, TestPriority(2)]
    public async Task Should_Get_Groups_By_UserId()
    {
        // Arrange
        var client = new GroupsGrpc.GroupsGrpcClient(_channel);

        // Act
        var response = await client.GetGroupsByUserIdAsync(new GetGroupsByUserIdRequest { Id = 1 });
        var result = _mapper.Map<IEnumerable<EntityGroup>>(response.Groups);

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

    [Fact, TestPriority(3)]
    public async Task Should_Be_Create_Group()
    {
        // Arrange
        var client = new GroupsGrpc.GroupsGrpcClient(_channel);
        var newGroup = new Group
        {
            Id = 3,
            Name = "CreateGroup"
        };

        // Act
        var response = await client.CreateGroupAsync(new CreateGroupRequest { Group = newGroup });
        var result = _mapper.Map<EntityGroup>(response.Group);

        // Assert
        result.Name.Should().Be(newGroup.Name);
    }

    [Fact, TestPriority(4)]
    public async Task Should_Be_Delete_Group()
    {
        // Arrange
        var client = new GroupsGrpc.GroupsGrpcClient(_channel);

        // Act
        var response = await client.DeleteGroupAsync(new DeleteGroupRequest { GroupId = 3 });
        var result = response.Result;

        // Assert
        result.Should().BeTrue();
    }

    [Fact, TestPriority(5)]
    public async Task Should_Get_Update_Group()
    {
        // Arrange
        var client = new GroupsGrpc.GroupsGrpcClient(_channel);
        var updateGroup = new Group
        {
            Id = 2,
            Name = "UpdateGroup"
        };

        // Act
        var response = await client.UpdateGroupAsync(new UpdateGroupRequest { Group = updateGroup });
        var result = _mapper.Map<EntityGroup>(response.Group);

        // Assert
        result.Id.Should().Be(2);
        result.Name.Should().Be(updateGroup.Name);
    }
}