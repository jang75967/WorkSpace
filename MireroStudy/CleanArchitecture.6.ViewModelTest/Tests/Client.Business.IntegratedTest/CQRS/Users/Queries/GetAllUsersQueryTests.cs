using AutoFixture;
using Client.Business.Core.Application.Features.Users.Queries;
using Client.Business.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore;
using Xunit;

namespace Client.Business.IntegratedTest.CQRS.Users.Queries;

public class GetAllUsersQueryTests : TestBase<ClientFactory<Program, ApplicationDbContext>>
{
    private readonly IFixture Fixture;
    public GetAllUsersQueryTests(ClientFactory<Program, ApplicationDbContext> factory)
    {
        Fixture = new Fixture().Customize(new ServiceProviderCustomization
        {
            Channel = factory.Channel
        }); ;
    }

    [Fact]
    public async Task Should_Be_Get_Users()
    {
        // Arrange
        var handler = Fixture.Create<GetAllUsersQueryHandler>();

        // Act
        var result = await handler.Handle(new GetAllUsersQuery());

        // Assert
        result.IsSome.Should().BeTrue();

        result.IfSome(x => 
        {
            x.Should().NotBeNull();
            x.Should().HaveCount(5);
        });
    }
}
