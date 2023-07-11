using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Extensions.Context;

namespace Infrastructure.MongoDB;

public static class MongoDBExtension
{
    public static IServiceCollection AddMongoDB(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddTransient<ApplicationDbContext>();
        service.Configure<MongoOptions>(configuration.GetSection("MongoDB"));
        service.AddSingleton(_ =>
        {
            var options = _.GetRequiredService<IOptions<MongoOptions>>();
            return new MongoOptions()
            {
                ConnectionString = options.Value.ConnectionString,
                DatabaseName = options.Value.DatabaseName,
            };
        });
        return service;
    }
}
