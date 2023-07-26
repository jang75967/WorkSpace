using Domain.Attributes;
using Domain.Exceptions;

namespace Domain.Entities;

[AggregateRoot]
public class User : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public User() { }

    public User(string name, string email, string password)
    {
        if (string.IsNullOrEmpty(name)) throw new DomainException($"{nameof(name)} is empty.");
        if (string.IsNullOrEmpty(email)) throw new DomainException($"{nameof(email)} is empty.");
        if (string.IsNullOrEmpty(password)) throw new DomainException($"{nameof(password)} is empty.");
        this.Name = name;
        this.Email = email;
        this.Password = password;
    }
    public static User Create(string name, string email, string password) => new User(name, email, password);

    public User Update(User user)
    {
        this.Name = user.Name;
        this.Email = user.Email;
        this.Password = user.Password;
        return this;
    }

    public User Update(string name, string email, string passwd)
    {
        this.Name = name;
        this.Email = email;
        this.Password = passwd;
        return this;
    }
}
