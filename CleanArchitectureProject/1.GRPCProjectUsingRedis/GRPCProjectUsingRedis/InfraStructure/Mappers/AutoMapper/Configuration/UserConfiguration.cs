using AutoMapper;
using DtoUser = Api.Users.User;
using EntityUser = Domain.Entities.User;

namespace InfraStructrue.Mappers.AutoMapper.Configuration
{
    public static class UserConfiguration
    {
        public static IMapperConfigurationExpression AddUser(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<EntityUser, DtoUser>().ReverseMap(); // two way

            //// Option<User> -> DtoUser 변환
            //cfg.CreateMap<Option<EntityUser>, DtoUser>()
            //    .ConvertUsing(optionUser => optionUser.Match(
            //        user => new DtoUser
            //        {
            //            Id = user.Id,
            //            Name = user.Name,
            //            Email = user.Email,
            //        },
            //        () => default!));

            //// IEnumerable<Option<EntityUser>> -> IEnumerable<DtoUser> 변환
            //cfg.CreateMap<IEnumerable<Option<EntityUser>>, IEnumerable<DtoUser>>()
            //    .ConvertUsing(optionUsers => optionUsers.Somes().Select(user => new DtoUser
            //    {
            //        Id = user.Id,
            //        Name = user.Name,
            //        Email = user.Email,
            //    }));

            return cfg;
        }
    }
}
