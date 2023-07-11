using Application.Mappers;
using Common;
using DotNet.Testcontainers.Containers;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanArchitecture.IntegratedTest.Factories;

public class TestDatabaseFactory<TProgram, TDbContext> :
    WebApplicationFactory<TProgram>,
    ITestFactory<TProgram, TDbContext>,
    IAsyncLifetime
     where TProgram : class where TDbContext : class
{
    private readonly IContainer _container;
    public GrpcChannel Channel => CreateChannel();
    public IMapper Mapper { get; set; } = default!;

    public TestDatabaseFactory(IContainer container)
    { 
        _container = container;
    }

    protected GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Server.CreateHandler(),
            MaxReceiveMessageSize = 1024 * 1024 * 1566,
            MaxSendMessageSize = 1024 * 1024 * 1566,
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        using var scope = Services.CreateScope();
        Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    }
    public new async Task DisposeAsync() => await _container.DisposeAsync();
}