using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Users.Queries;
using CleanArchitecture.UnitTest.EFCore.Users.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using FluentValidation.TestHelper;
using Infrastructure.EFCore.Repositories;
using Xunit;

namespace CleanArchitecture.UnitTest.EFCore.Users.Queries;

public class GetUserByIdQueryTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public GetUserByIdQueryTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_User_Count_Is_Five()
    {
        // Arrange
        var userRepository = new UserRepository(new MockDbContext().Get().Object);
        var handler = new GetUserByIdQueryHandler(userRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetUserByIdQuery(1));

        // Assert
        result.Name.Should().Be("박영석");
        result.Email.Should().Be("bak@gmail.com");
    }

    [Fact]
    public void Should_Be_Expect_Exception_By_GreaterThanValidator()
    {
        // Arrange
        var validator = new GetUserByIdValidator();
        var query = new GetUserByIdQuery(0);

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(query => query.UserId).WithErrorCode("GreaterThanValidator");
    }
}
