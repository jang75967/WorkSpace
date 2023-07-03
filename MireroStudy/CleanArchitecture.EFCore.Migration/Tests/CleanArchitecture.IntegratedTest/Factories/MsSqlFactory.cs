using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Testcontainers.MsSql;
using Xunit;

namespace CleanArchitecture.IntegratedTest.Factories;

public class MsSqlFactory<TProgram, TDbContext> : TestDatabaseFactory<TProgram, TDbContext> where TProgram : class where TDbContext : DbContext
{
    private static MsSqlContainer _container = new MsSqlBuilder().Build();

    public MsSqlFactory() : base(_container) { }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<TDbContext>();
            services.AddDbContextFactory<TDbContext>(option =>
            {
                var url = _container.GetConnectionString();
                option.UseSqlServer(url,
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            });
            services.EnsureDbCreated<TDbContext>();
        });
    }
}
