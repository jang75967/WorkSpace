using Api.Activities;
using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Activities.Commands;
using CleanArchitecture.UnitTest.EFCore.Activities.Mocks;
using CleanArchitecture.UnitTest.Factories;
using Common;
using FluentAssertions;
using Infrastructure.EFCore.Repositories;
using Moq;
using Xunit;
using DtoExpense = Api.Activities.Expense;

namespace CleanArchitecture.UnitTest.EFCore.Activities.Commands;

public class RemoveExpenseCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public RemoveExpenseCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Remove_Expense()
    {
        var request = new RemoveExpenseRequest
        {
            ActivityId = 1,
            Expense = new DtoExpense { Id = 1 }
        };

        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var activityRepository = new ActivityRepository(mockDbContext.Object);
        var handler = new RemoveExpenseCommandHandler(activityRepository, _mapper);

        // Act
        var result = await handler.Handle(new RemoveExpenseCommand(request));

        // Assert
        result.Id.Should().Be(1);
        result.Expenses.Count().Should().Be(1);

        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}