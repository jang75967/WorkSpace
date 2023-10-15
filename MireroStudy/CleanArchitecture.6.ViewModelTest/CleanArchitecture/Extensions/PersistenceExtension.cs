using Application.Persistences;
using Infrastructure.EFCore;
using System.Reflection;

namespace CleanArchitecture.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
    {
        var inMemory = configuration.GetSection("In-Memory").Value;
        var IsInMemory = inMemory != null ? Convert.ToBoolean(inMemory.ToLower()) : false;

        if (IsInMemory)
        {
            services.AddInMemory(Assembly.GetExecutingAssembly().GetName().Name!);

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }
        }
        else
        {
            services.AddPostgreSql(configuration, Assembly.GetExecutingAssembly().GetName().Name!);
        }


        //services.AddOracle(configuration);
        //services.AddMsSql(configuration);
        //services.AddMongoDB(configuration);
        return services;
    }
}
