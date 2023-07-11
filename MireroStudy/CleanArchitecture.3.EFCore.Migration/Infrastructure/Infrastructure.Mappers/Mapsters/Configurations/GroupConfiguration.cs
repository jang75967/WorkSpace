using Mapster;
using DtoGroup = Api.Users.Group;
using EntityGroup = Domain.Entities.Group;

namespace Infrastructure.Mappers.Mapsters.Configurations;

public static class GroupConfiguration
{
    public static TypeAdapterConfig AddGroup(this TypeAdapterConfig cfg)
    {
        cfg.NewConfig<EntityGroup, DtoGroup>().PreserveReference(true);
        return cfg;
    }
}
