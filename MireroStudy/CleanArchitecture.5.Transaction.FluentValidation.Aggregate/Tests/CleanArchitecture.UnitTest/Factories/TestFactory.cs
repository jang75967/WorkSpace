using Application.Mappers;
using CleanArchitecture.Extensions;
using Grpc.Net.Client;
using Infrastructure.Mappers.AutoMappers;
using Infrastructure.Mappers.Mapsters;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanArchitecture.UnitTest.Factories;

public class TestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    public IMapper Mapper { get; set; } = default!;
    public IMediator Mediator { get; set; }
    public TestFactory() { }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddAutoMapper();
            //services.AddMapster();
        });
    }

    public Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
    }
}