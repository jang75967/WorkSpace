using Infrastructure.MongoDB.EntityConfigurations;
using MongoDB.Extensions.Context;

namespace Infrastructure.MongoDB;

public class ApplicationDbContext : MongoDbContext
{
    public ApplicationDbContext(MongoOptions options) : base(options) { }

    protected override void OnConfiguring(IMongoDatabaseBuilder mongoDatabaseBuilder)
    {
        mongoDatabaseBuilder
            .ConfigureCollection(new BaseEntityConfiguration())
            .ConfigureCollection(new UserConfiguration());
    }
}
