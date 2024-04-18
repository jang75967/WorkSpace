using Application.Persistences;
using InfraStructrue.Data.Repositories;

namespace WorkerService.Extensions
{
    public static class ReositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, FakeUserRepsitory>();

            return services;
        }
    }
}
