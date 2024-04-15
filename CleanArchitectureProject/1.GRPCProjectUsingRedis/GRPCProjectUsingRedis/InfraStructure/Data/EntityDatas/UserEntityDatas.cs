using Domain.Entities;
using LanguageExt;
using static LanguageExt.Prelude;

namespace InfraStructure.Data.EntityDatas
{
    public static class UserEntityDatas
    {
        public static IEnumerable<Option<User>> Initialize()
        {
            return new List<Option<User>>()
            {
                Some(new User
                {
                    Id = 1,
                    Name = "jdg1",
                    Email = "jdg1@gmail.com",
                }),
                Some(new User
                {
                    Id = 2,
                    Name = "jdg2",
                    Email = "jdg2@gmail.com",
                }),
                Some(new User
                {
                    Id = 3,
                    Name = "jdg3",
                    Email = "jdg3@gmail.com",
                }),
                None,
            };
        }
    }
}
