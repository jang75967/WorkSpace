using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Users.Commands;
using CleanArchitecture.Core.Application.Features.Users.Queries;
using CleanArchitecture.UnitTest.EFCore.Users.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using FluentValidation.TestHelper;
using Infrastructure.EFCore.Repositories;
using Moq;
using System.Collections;
using System.Configuration;
using Xunit;
using DtoUser = Api.Users.User;

namespace CleanArchitecture.UnitTest.EFCore.Users.Commands;

public class CreateUserCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public CreateUserCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Create_Group()
    {
        var userDto = new DtoUser { Id = 6, Name = "created", Email = "created@gmail.com", Password ="passwd" };

        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var userRepository = new UserRepository(mockDbContext.Object);
        var handler = new CreateUserCommandHandler(userRepository, _mapper);

        // Act
        var result = await handler.Handle(new CreateUserCommand(userDto));

        // Assert
        result.Name.Should().Be(userDto.Name);
        result.Email.Should().Be(userDto.Email);
        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    public static IEnumerable<object[]> GetUserNameTests()
    {
        yield return new object[] { new DtoUser { Id = 0, Name = "" } , "NotEmptyValidator" };
        yield return new object[] { new DtoUser { Id = 0, Name = "".PadRight(40) } , "LengthValidator" };
    }

    [Theory]
    [MemberData(nameof(GetUserNameTests))]
    public void Should_Be_Expect_Exception_By_User_Name(DtoUser dtoUser, string errorCode)
    {
        // Arrange
        var validator = new CreateUserValidator();
        var query = new CreateUserCommand(dtoUser);

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(query => query.User.Name).WithErrorCode(errorCode);
    }
}