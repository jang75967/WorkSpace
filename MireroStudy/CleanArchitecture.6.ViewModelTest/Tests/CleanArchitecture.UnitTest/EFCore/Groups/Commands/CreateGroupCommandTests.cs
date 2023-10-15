using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Groups.Commands;
using CleanArchitecture.UnitTest.EFCore.Groups.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Moq;
using Xunit;
using DtoGroup = Api.Groups.Group;

namespace CleanArchitecture.UnitTest.EFCore.Groups.Commands;

public  class CreateGroupCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public CreateGroupCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Create_Group()
    {
        var groupDto = new DtoGroup { Id = 3, Name = "created" };

        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var groupRepository = new GroupRepository(mockDbContext.Object);
        var handler = new CreateGroupCommandHandler(groupRepository, _mapper);

        // Act
        var result = await handler.Handle(new CreateGroupCommand(groupDto));

        // Assert
        result.Name.Should().Be(groupDto.Name);
        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}