using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Groups.Commands;
using CleanArchitecture.UnitTest.EFCore.Groups.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DtoGroup = Api.Groups.Group;

namespace CleanArchitecture.UnitTest.EFCore.Groups.Commands;

public class DeleteGroupCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public DeleteGroupCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Delete_Group()
    {
        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var groupRepository = new GroupRepository(mockDbContext.Object);
        var handler = new DeleteGroupCommandHandler(groupRepository, _mapper);

        // Act
        var result = await handler.Handle(new DeleteGroupCommand(groupId: 2));

        // Assert
        result.Should().BeTrue();
        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}