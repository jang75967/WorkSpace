using Api.Activities;
using Application.Mappers;
using CleanArchitecture.IntegratedTest.Factories;
using Common;
using FluentAssertions;
using Grpc.Net.Client;
using Infrastructure.EFCore;
using Xunit;
using DtoAttendant = Api.Activities.Attendant;
using DtoExpense = Api.Activities.Expense;
using EntityActivity = Domain.Entities.ActivityAggregate.Activity;

namespace CleanArchitecture.IntegratedTest;

[TestCaseOrderer(ordererTypeName: "Common.PriorityOrderer", ordererAssemblyName: "Common")]
public class ActivityControllerTests : TestBase<PostgresFactory<Program, ApplicationDbContext>>
{
    private readonly GrpcChannel _channel;
    private readonly IMapper _mapper;
    public ActivityControllerTests(PostgresFactory<Program, ApplicationDbContext> factory)
    {
        _channel = factory.Channel;
        _mapper = factory.Mapper;
    }

    [Fact, TestPriority(1)]
    public async Task Should_Be_Create_Activity()
    {
        // Arrange
        var client = new ActivitiesGrpc.ActivitiesGrpcClient(_channel);
        var title = "가을운동회";
        var groupId = 1;
        var attendees = new List<DtoAttendant> {new DtoAttendant { UserId = 3}, new DtoAttendant { UserId = 4}};
        var expenses = new List<DtoExpense>{new DtoExpense { Payment = 20000 },new DtoExpense { Payment = 30000 }};

        // Act
        var response = await client.CreateActivityAsync(new CreateActivityRequest() {
            Title = title,
            GroupId = groupId,
            Attendees = { attendees },
            Expenses = { expenses }
        });
        var result = _mapper.Map<EntityActivity>(response.Activity);

        // Assert
        result.Id.Should().Be(3);
        result.GroupId.Should().Be(1);
        result.Title.Should().Be("가을운동회");
        result.Attendees.Count.Should().Be(2);
        result.Expenses.Count.Should().Be(2);
    }

    [Fact, TestPriority(2)]
    public async Task Should_Be_Add_Expense()
    {
        // Arrange
        var client = new ActivitiesGrpc.ActivitiesGrpcClient(_channel);

        // Act
        var response = await client.AddExpenseAsync(new AddExpenseRequest()
        {
            ActivityId = 1,
            Expense = new DtoExpense { Payment = 20000 }
        });
        var result = _mapper.Map<EntityActivity>(response.Activity);

        // Assert
        result.TotalPayment.Should().Be(40000);
        result.Title.Should().Be("체육대회");
        result.GroupId.Should().Be(1);
        result.Attendees.Count.Should().Be(2);
        result.Expenses.Count.Should().Be(3);
    }

    [Fact, TestPriority(3)]
    public async Task Should_Be_Remove_Expense()
    {
        // Arrange
        var client = new ActivitiesGrpc.ActivitiesGrpcClient(_channel);

        // Act
        var response = await client.RemoveExpenseAsync(new RemoveExpenseRequest()
        {
            ActivityId = 2,
            Expense = new DtoExpense { Id = 3 }
        });
        var result = _mapper.Map<EntityActivity>(response.Activity);

        // Assert
        result.Id.Should().Be(2);
        result.TotalPayment.Should().Be(20000);
        result.Attendees.Count.Should().Be(4);
        result.Expenses.Count.Should().Be(1);
    }

    [Fact, TestPriority(4)]
    public async Task Should_Be_Update_Expense()
    {
        // Arrange
        var client = new ActivitiesGrpc.ActivitiesGrpcClient(_channel);

        // Act
        var response = await client.UpdateExpenseAsync(new UpdateExpenseRequest()
        {
            ActivityId = 2,
            Expense = new DtoExpense { Id = 4, Payment = 999 }
        });
        var result = _mapper.Map<EntityActivity>(response.Activity);

        // Assert
        result.Id.Should().Be(2);
        result.TotalPayment.Should().Be(999);
        result.Expenses.Where(x=>x.Id==4).First().Payment.Should().Be(999);
    }
}
