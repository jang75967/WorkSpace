using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Testcontainers.Oracle;

namespace CleanArchitecture.IntegratedTest.Factories;

public class OracleFactory<TProgram, TDbContext> : TestDatabaseFactory<TProgram, TDbContext> where TProgram : class where TDbContext : DbContext
{
    private static OracleContainer _container = new OracleBuilder()
            .WithEnvironment("APP_USER", "DM80")
            .WithUsername("DM80")
            .WithPassword("DM80")
            .Build();

    public OracleFactory() : base(_container) { }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<TDbContext>();
            services.AddDbContextFactory<TDbContext>(option =>
            {
                var url = _container.GetConnectionString();
                option.UseOracle(url,
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            });
            services.EnsureDbCreated<TDbContext>();
        });
    }
}
