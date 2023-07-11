using Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Extensions.Context;

namespace Infrastructure.MongoDB.EntityConfigurations;

[BsonIgnoreExtraElements]
public class BaseEntityConfiguration : IMongoCollectionConfiguration<BaseEntity>
{
    public void OnConfiguring(IMongoCollectionBuilder<BaseEntity> builder)
    {
        builder.AddBsonClassMap<BaseEntity>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);
            classMap.MapIdMember(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
        });
    }
}