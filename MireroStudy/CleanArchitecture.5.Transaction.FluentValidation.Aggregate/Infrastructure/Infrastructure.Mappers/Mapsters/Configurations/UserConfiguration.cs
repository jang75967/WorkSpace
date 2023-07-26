using Mapster;
using EntityUser = Domain.Entities.User;
using DtoUser = Api.Users.User;

namespace Infrastructure.Mappers.Mapsters.Configurations;

public static class UserConfiguration
{
    public static TypeAdapterConfig AddUser(this TypeAdapterConfig cfg)
    {
        cfg.NewConfig<EntityUser, DtoUser>().PreserveReference(true);
        return cfg;
    }
}
