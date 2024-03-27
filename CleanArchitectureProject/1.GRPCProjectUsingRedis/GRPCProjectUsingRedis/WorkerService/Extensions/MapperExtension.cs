using InfraStructrue.Mappers.AutoMapper;

namespace WorkerService.Extensions
{
    public static class MapperExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper();

            return services;
        }
    }
}
