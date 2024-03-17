using Domain.Entities;
using LanguageExt;

namespace Application
{
    public interface IUserService
    {
        public Task<IEnumerable<Option<User>>> GetUsers();
    }
}
