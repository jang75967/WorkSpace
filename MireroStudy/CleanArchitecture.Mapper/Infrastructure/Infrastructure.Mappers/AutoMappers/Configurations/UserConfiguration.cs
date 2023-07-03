using AutoMapper;
using EntityUser = Domain.Entities.User;
using DtoUser = Api.Users.User;

namespace Infrastructure.Mappers.AutoMappers.Configurations;

public static class UserConfiguration
{
    public static IMapperConfigurationExpression AddUser(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<EntityUser, DtoUser>().ReverseMap();
        return cfg;
    }
}
