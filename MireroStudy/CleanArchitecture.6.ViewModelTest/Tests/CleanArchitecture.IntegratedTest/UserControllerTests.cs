using Api.Users;
using Application.Mappers;
using CleanArchitecture.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Infrastructure.EFCore;
using Xunit;
using EntityUser = Domain.Entities.User;

namespace CleanArchitecture.IntegratedTest;

[TestCaseOrderer(ordererTypeName: "Common.PriorityOrderer", ordererAssemblyName: "Common")]
public class UserControllerTests : TestBase<PostgresFactory<Program, ApplicationDbContext>>
{
    private readonly GrpcChannel _channel;
    private readonly IMapper _mapper;
    public UserControllerTests(PostgresFactory<Program, ApplicationDbContext> factory)
    {
        _channel = factory.Channel;
        _mapper = factory.Mapper;
    }

    [Fact, TestPriority(0)]
    public async Task Should_Be_User_Count_Is_Five()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetAllUsersAsync(new GetAllUsersRequest { });
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

    [Fact, TestPriority(1)]
    public async Task Should_Get_User_By_UserId()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetUserByIdAsync(new GetUserByIdRequest { Id = 1 });
        var result = _mapper.Map<EntityUser>(response.User);

        // Assert
        result.Id.Should().Be(1);
        result.Name.Should().Be("박영석");
        result.Email.Should().Be("bak@gmail.com");
    }

    [Fact, TestPriority(2)]
    public async Task Should_Get_Users_By_GroupId()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        // Act
        var response = await client.GetMemebersByGroupIdAsync(new GetMemebersByGroupIdRequest { Id = 1 });
        var result = _mapper.Map<IEnumerable<EntityUser>>(response.Users);

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
                third.Name.Should().Be("안성윤");
                third.Email.Should().Be("an@gmail.com");
            });
    }

    [Fact, TestPriority(3)]
    public async Task Should_Be_Create_User()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);

        var newUser = new User() { Name = "CreateUser", Password = "password", Email = "new@gmail.com" };

        // Act
        var response = await client.CreateUserAsync(new CreateUserRequest { User = newUser });
        var result = _mapper.Map<EntityUser>(response.User);

        // Assert
        result.Name.Should().Be(newUser.Name);
    }

    [Fact, TestPriority(4)]
    public async Task Should_Be_Delete_User()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);
        
        // Act
        var response = await client.DeleteUserAsync(new DeleteUserRequest { UserId = 6 });
        var result = response.Result;

        // Assert
        result.Should().BeTrue();
    }

    [Fact, TestPriority(5)]
    public async Task Should_Get_Update_User()
    {
        // Arrange
        var client = new UsersGrpc.UsersGrpcClient(_channel);
        var updateUser = new User
        {
            Id = 1,
            Name = "UpdateUser",
            Email="UpdateEmail",
            Password = "UpdatePassword",
        };

        // Act
        var response = await client.UpdateUserAsync(new UpdateUserRequest { User = updateUser });
        var result = _mapper.Map<EntityUser>(response.User);

        // Assert
        result.Id.Should().Be(1);
        result.Name.Should().Be(updateUser.Name);
    }
}