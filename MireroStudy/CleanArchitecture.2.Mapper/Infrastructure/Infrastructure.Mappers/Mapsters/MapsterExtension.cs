using Infrastructure.Mappers.Mapsters.Configurations;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mappers.Mapsters;

public static class MapsterExtension
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        services.AddSingleton<MapsterMapper.IMapper>(MapperBuilder());
        services.AddSingleton<Application.Mappers.IMapper, MapsterDI>();
        return services;
    }

    private static MapsterMapper.Mapper MapperBuilder()
    {
        var config = new TypeAdapterConfig();
        config.AddUser();
        return new MapsterMapper.Mapper(config);
    }
}
