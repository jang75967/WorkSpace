using Domain.Attributes;
using Domain.Exceptions;

namespace Domain.Entities;

[AggregateRoot]
public class Group : BaseEntity
{
    public string Name { get; set; } = default!;

    public Group() { }

    public Group(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new DomainException($"{nameof(name)} is empty.");
        this.Name = name;
    }
    public static Group Create(string name) => new Group(name);

    public Group Update(Group user)
    {
        this.Name = user.Name;
        return this;
    }
}
