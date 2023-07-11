using Application.Mappers;
using Grpc.Net.Client;
using Infrastructure.Mappers.Mapsters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanArchitecture.UnitTest.Factories;

public class TestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    public GrpcChannel Channel => CreateChannel();
    public IMapper Mapper { get; set; } = default!;
    public TestFactory() { }

    protected GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Server.CreateHandler(),
            MaxReceiveMessageSize = 1024 * 1024 * 1566,
            MaxSendMessageSize = 1024 * 1024 * 1566,
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddMapster();
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