using Application;
using InfraStructrue.Data.Repositories;

namespace WorkerService.Extensions
{
    public static class ReositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, FakeRepsitory>();

            return services;
        }
    }
}
