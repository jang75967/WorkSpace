using Application.Persistences;
using Domain.Entities;
using Infrastructure.MongoDB;
using Infrastructure.Persistence.Common.EntityDatas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Extensions.Context;
using Testcontainers.MongoDb;

namespace CleanArchitecture.IntegratedTest.Factories;

public class MongoDBFactory<TProgram, TDbContext> : TestDatabaseFactory<TProgram, TDbContext> where TProgram : class where TDbContext : MongoDbContext
{
    private static MongoDbContainer _container = new MongoDbBuilder().Build();

    public MongoDBFactory() : base(_container) { }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<IUserRepository, Infrastructure.MongoDB.Repositories.UserRepository>();
            services.AddTransient<ApplicationDbContext>();
            services.AddSingleton(_ =>
            {
                var conn = _container.GetConnectionString();
                return new MongoOptions()
                {
                    ConnectionString = _container.GetConnectionString(),
                    DatabaseName = "DM80"
                };
            });
        });
        DataInitialize();
    }

    private void DataInitialize()
    {
        var client = new MongoClient(new MongoUrl(_container.GetConnectionString()));
        var db = client.GetDatabase("DM80");
        var collection = db.GetCollection<User>(nameof(User));
        collection.InsertMany(UserEntityDatas.Initialize());
    }
}
