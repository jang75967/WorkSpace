using AutoMapper;
using EntityGroup = Domain.Entities.Group;
using DtoGroup = Api.Users.Group;

namespace Infrastructure.Mappers.AutoMappers.Configurations;

public static class GroupConfiguration
{
    public static IMapperConfigurationExpression AddGroup(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<EntityGroup, DtoGroup>().ReverseMap();
        return cfg;
    }
}

