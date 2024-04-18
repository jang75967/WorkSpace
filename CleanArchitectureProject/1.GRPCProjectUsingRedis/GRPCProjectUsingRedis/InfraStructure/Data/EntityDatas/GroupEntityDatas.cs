using Domain.Entities;
using LanguageExt;
using static LanguageExt.Prelude;

namespace InfraStructure.Data.EntityDatas
{
    public class GroupEntityDatas
    {
        public static IEnumerable<Option<Group>> Initialize()
        {
            return new List<Option<Group>>()
            {
                Some(new Group
                {
                    Id = 1,
                    Name = "축구"
                }),
                Some(new Group
                {
                    Id= 2,
                    Name = "농구"
                }),
                Some(new Group
                {
                    Id = 3,
                    Name = "야구"
                })
            };
        }
    }
}
