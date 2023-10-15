using AutoMapper;
using Client.Business.Core.Domain.Models.Users;
using DtoUser = Api.Users.User;

namespace Client.Business.Infrastructure.Configurations;

public static class UserConfiguration
{
    public static IMapperConfigurationExpression AddUser(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<UserModel, DtoUser>().ReverseMap();
        return cfg;
    }
}