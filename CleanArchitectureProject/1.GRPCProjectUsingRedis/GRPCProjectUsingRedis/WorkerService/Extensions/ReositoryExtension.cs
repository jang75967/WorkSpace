using Application.Persistences;
using Domain.Entities;
using InfraStructrue.Data.Repositories;
using InfraStructure.Data.Persistence.EFCore;
using InfraStructure.Data.Repositories;

namespace WorkerService.Extensions
{
    public static class ReositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //services.AddSingleton<IUserRepository, FakeUserRepsitory>();

            services.AddScoped<IBaseRepository<User>, EFCoreRepository<User>>();
            services.AddScoped<IBaseRepository<Group>, EFCoreRepository<Group>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
