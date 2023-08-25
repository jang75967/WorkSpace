using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Users.Commands;
using CleanArchitecture.UnitTest.EFCore.Users.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Moq;
using Xunit;
using DtoUser = Api.Users.User;
namespace CleanArchitecture.UnitTest.EFCore.Users.Commands;

public class UpdateUserCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public UpdateUserCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Get_Update_User()
    {
        var userDto = new DtoUser { Id = 1, Name = "updated", Email = "updated@gmail.com" };

        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var userRepository = new UserRepository(mockDbContext.Object);
        var handler = new UpdateUserCommandHandler(userRepository, _mapper);

        // Act
        var result = await handler.Handle(new UpdateUserCommand(userDto));

        // Assert
        result.Name.Should().Be(userDto.Name);
        result.Email.Should().Be(userDto.Email);
        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
