using Domain.Entities;

namespace InfraStructure.Data.EntityDatas
{
    public static class GroupEntityDatas
    {
        public static IEnumerable<Group> Initialize()
        {
            return new List<Group>()
            {
                new()
                {
                    Id = 1,
                    Name = "축구"
                },
                new()
                {
                    Id= 2,
                    Name = "농구"
                },
                new()
                {
                    Id = 3,
                    Name = "야구"
                }
            };
        }
    }
}
