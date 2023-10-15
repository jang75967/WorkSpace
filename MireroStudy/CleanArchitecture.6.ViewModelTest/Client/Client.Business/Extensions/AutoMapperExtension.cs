using Client.Business.Infrastructure;
using Client.Business.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Business.Extensions;

public static class AutoMapperExtension
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddSingleton<Application.Mappers.IMapper, AutoMapperDI>();
        services.AddSingleton<AutoMapper.IMapper>(new AutoMapper.Mapper(MapperBuilder()));
        return services;
    }

    private static AutoMapper.MapperConfiguration MapperBuilder()
    {
        return new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddUser();
        });
    }
}
