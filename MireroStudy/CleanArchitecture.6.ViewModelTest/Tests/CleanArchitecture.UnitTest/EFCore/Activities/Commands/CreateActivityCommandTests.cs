using Application.Mappers;
using CleanArchitecture.Core.Application.Features.Users.Commands;
using CleanArchitecture.UnitTest.Factories;
using Common;
using Infrastructure.EFCore.Repositories;
using Moq;
using DtoActivity = Api.Activities.Activity;
using DtoAttendant = Api.Activities.Attendant;
using DtoExpense = Api.Activities.Expense;
using Xunit;
using CleanArchitecture.UnitTest.EFCore.Activities.Mocks;
using CleanArchitecture.Core.Application.Features.Activities.Commands;
using Application.Persistences;
using FluentAssertions;
using Api.Activities;

namespace CleanArchitecture.UnitTest.EFCore.Activities.Commands;

public class CreateActivityCommandTests : TestBase<TestFactory<Program>>
{
    private readonly IMapper _mapper;
    public CreateActivityCommandTests(TestFactory<Program> factory)
    {
        _mapper = factory.Mapper;
    }

    [Fact]
    public async Task Should_Be_Create_Activity()
    {
        var request = new CreateActivityRequest()
        {
            Title = "가을운동회",
            GroupId = 1,
            Attendees = { new List<DtoAttendant> { new DtoAttendant { UserId = 3 }, new DtoAttendant { UserId = 4 } }},
            Expenses = { new List<DtoExpense> { new DtoExpense { Payment = 20000 }, new DtoExpense { Payment = 30000 } }}
        };
        // Arrange
        var mockDbContext = new MockDbContext().Get();
        var activityRepository = new ActivityRepository(mockDbContext.Object);
        var handler = new CreateActivityCommandHandler(activityRepository, _mapper);

        // Act
        var result = await handler.Handle(new CreateActivityCommand(request));

        // Assert
        result.Title.Should().Be("가을운동회");
        
        mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}