using Application;
using AutoMapper;
using InfraStructrue.Mappers.AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfraStructrue.Mappers.AutoMapper
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<ICustomMapper, AutoMapperCustom>();
            services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));

            return services;
        }

        private static MapperConfiguration GetMapperConfiguration()
        {
            return new MapperConfiguration(cfg => cfg.AddUser());
        }
    }
}
