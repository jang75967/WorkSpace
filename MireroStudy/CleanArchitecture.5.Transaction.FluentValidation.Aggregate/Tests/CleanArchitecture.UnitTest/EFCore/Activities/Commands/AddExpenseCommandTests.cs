using Api.Activities;
using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Activities.Commands;
using CleanArchitecture.UnitTest.Factories;
using Common;
using Infrastructure.EFCore.Repositories;
using Moq;
using DtoExpense = Api.Activities.Expense;
using Xunit;
using CleanArchitecture.UnitTest.EFCore.Activities.Mocks;
using FluentAssertions;

namespace CleanArchitecture.UnitTest.EFCore.Activities.Commands;

public class AddExpenseCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public AddExpenseCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Add_Expense()
    {
        var request = new AddExpenseRequest
        {
            ActivityId = 1,
            Expense = new DtoExpense { Payment = 20000 }
        };

        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var activityRepository = new ActivityRepository(mockDbContext.Object);
        var handler = new AddExpenseCommandHandler(activityRepository, _mapper);

        // Act
        var result = await handler.Handle(new AddExpenseCommand(request));

        // Assert
        result.Id.Should().Be(1);
        result.Expenses.Count().Should().Be(3);

        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}