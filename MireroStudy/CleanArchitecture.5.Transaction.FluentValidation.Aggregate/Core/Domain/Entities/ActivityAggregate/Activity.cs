using Domain.Attributes;
using Domain.Exceptions;
using System.Reflection;

namespace Domain.Entities.ActivityAggregate;

[AggregateRoot]
public class Activity : BaseEntity
{
    public string Title { get; set; } = default!;
    public long GroupId { get; set; } = default!;
    public float TotalPayment { get; set; } = default!;
    public Group Group { get; set; } = default!;
    public ICollection<Expense> Expenses { get; set; } = default!;
    public ICollection<Attendant> Attendees { get; set; } = default!;

    public Activity() { }

    public Activity(string title, long groupId)
    {
        if(string.IsNullOrEmpty(title)) throw new DomainException($"{nameof(title)} is empty.");
        if (groupId <= 0) throw new DomainException($"Invalid groupId: '{nameof(groupId)}'. GroupId must be a positive number.");

        this.Title = title;
        this.GroupId = groupId;
        Expenses = new List<Expense>();
        Attendees = new List<Attendant>();
    }

    public static Activity Create(string title, long groupId) => new Activity(title, groupId);

    public Expense AddExpense(Expense expense)
    {
        expense.Activity = this;
        expense.ActivityId = this.Id;
        this.Expenses.Add(expense);
        TotalPayment = this.Expenses.Select(x => x.Payment).Sum();
        return expense;
    }

    public Attendant AddAttendant(Attendant attendant)
    {
        attendant.Activity = this;
        attendant.ActivityId = this.Id;
        this.Attendees.Add(attendant);
        return attendant;
    }

    public void RemoveExpense(Expense expense)
    {
        var target = this.Expenses.Where(x => x.Id == expense.Id).FirstOrDefault();
        if (target == null)
            throw new DomainException("Expense with the specified ID not found");
        this.Expenses.Remove(target);
        TotalPayment = this.Expenses.Select(x => x.Payment).Sum();
    }

    public void RemoveAttendant(Attendant attendant)
    {
        var target = this.Attendees.Where(x => x.Id == attendant.Id).FirstOrDefault();
        if (target == null)
            throw new DomainException("Attendant with the specified ID not found");
        this.Attendees.Remove(target);
    }

    public Expense UpdateExpense(Expense expense)
    {
        var target = this.Expenses.Where(x => x.Id == expense.Id).FirstOrDefault();
        if (target == null)
            throw new DomainException("Expense with the specified ID not found");

        target.Payment = expense.Payment;
        TotalPayment = this.Expenses.Select(x => x.Payment).Sum();
        return target;
    }
}

