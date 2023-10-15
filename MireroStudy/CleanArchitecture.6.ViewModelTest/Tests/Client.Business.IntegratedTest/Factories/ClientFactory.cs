using AutoFixture;
using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Testcontainers.PostgreSql;

namespace Client.Business.IntegratedTest.Factories;

public class ClientFactory<TProgram, TDbContext> : TestDatabaseFactory<TProgram, TDbContext> where TProgram : class where TDbContext : DbContext
{

    public ClientFactory() : base(new PostgreSqlBuilder().Build()){ }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<TDbContext>();
            services.AddDbContextFactory<TDbContext>(option =>
            {
                var url = (Container as PostgreSqlContainer)!.GetConnectionString();
                option.UseNpgsql(url,
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            });
            services.EnsureDbCreated<TDbContext>();
        });
    }
}
