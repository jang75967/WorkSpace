﻿using CleanArchitecture.Core.Application.Features.Groups.Commands;
using CleanArchitecture.UnitTest.Factories;
using Common;
using Infrastructure.EFCore.Repositories;
using Moq;
using DtoUser = Api.Users.User;
using Xunit;
using Application.Mappers;
using CleanArchitecture.UnitTest.EFCore.Users.Mocks;
using CleanArchitecture.Core.Application.Features.Users.Commands;
using FluentAssertions;

namespace CleanArchitecture.UnitTest.EFCore.Users.Commands;

public class DeleteUserCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public DeleteUserCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Create_Group()
    {
        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var userRepository = new UserRepository(mockDbContext.Object);
        var handler = new DeleteUserCommandHandler(userRepository, _mapper);

        // Act
        var result = await handler.Handle(new DeleteUserCommand(userId: 1));

        // Assert
        result.Should().BeTrue();
        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}