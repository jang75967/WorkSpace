using Infrastructure.Mappers.AutoMappers;
using Infrastructure.Mappers.Mapsters;
namespace CleanArchitecture.Extensions;

public static class MapperExtension
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        //services.AddAutoMapper();
        services.AddMapster();
        return services;
    }
}
