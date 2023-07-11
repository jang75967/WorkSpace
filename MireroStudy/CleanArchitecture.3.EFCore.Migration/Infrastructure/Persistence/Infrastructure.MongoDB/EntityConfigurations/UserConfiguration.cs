using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Extensions.Context;

namespace Infrastructure.MongoDB.EntityConfigurations;

public class UserConfiguration : IMongoCollectionConfiguration<User>
{
    public void OnConfiguring(IMongoCollectionBuilder<User> builder)
    {
        builder.AddBsonClassMap<User>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);
            classMap.MapIdMember(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance).SetSerializer(new StringSerializer(BsonType.ObjectId));
        });
    }
}
