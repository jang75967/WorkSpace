using Application.Persistences;

namespace CleanArchitecture.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Postgres
        services.AddScoped<IUserRepository, Infrastructure.EFCore.Repositories.UserRepository>();
        services.AddScoped<IGroupRepository, Infrastructure.EFCore.Repositories.GroupRepository>();

        // MongoDB
        //services.AddScoped<IUserRepository, Infrastructure.MongoDB.Repositories.UserRepository>();
        return services;
    }
}

