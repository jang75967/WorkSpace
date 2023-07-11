using Api.Users;
using Application.Mappers;
using CleanArchitecture.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Infrastructure.EFCore;
using Xunit;
using EntityUser = Domain.Entities.User;

namespace CleanArchitecture.IntegratedTest.UserServiceTests;

public class UserServiceInPostgresTests : TestBase<PostgresFactory<Program, ApplicationDbContext>>
{
    private readonly GrpcChannel _channel;
    private readonly IMapper _mapper;
    public UserServiceInPostgresTests(PostgresFactory<Program, ApplicationDbContext> factory)
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
                first.Name.Should().Be("�ڿ���");
                first.Email.Should().Be("bak@gmail.com");
            },
            second =>
            {
                second.Name.Should().Be("�ȼ���");
                second.Email.Should().Be("an@gmail.com");

            },
            third =>
            {
                third.Name.Should().Be("�̰ǿ�");
                third.Email.Should().Be("lee@gmail.com");

            },
            fourth =>
            {
                fourth.Name.Should().Be("�嵿��");
                fourth.Email.Should().Be("jang@gmail.com");
            },
            fifth =>
            {
                fifth.Name.Should().Be("������");
                fifth.Email.Should().Be("jo@gmail.com");
            });
    }
}