using Application.Mappers;
using Application.Persistences;
using Domain.Entities;
using Grpc.Net.Client;
using Infrastructure.Mappers.Mapsters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
            services.AddSingleton<IUserRepository>(MockUserRepository());
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

    private IUserRepository MockUserRepository()
    {
        var mock = new Mock<IUserRepository>();
        mock.Setup(_ => _.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>()
                {
                    new User() { Id=1, Name = "박영석", Password = "password", Email ="bak@gmail.com"},
                    new User() { Id=2, Name = "이건우", Password = "password", Email ="lee@gmail.com"},
                    new User() { Id=3, Name = "조범희", Password = "password", Email ="jo@gmail.com"},
                    new User() { Id=4, Name = "안성윤", Password = "password", Email ="an@gmail.com"},
                    new User() { Id=5, Name = "장동계", Password = "password", Email ="jang@gmail.com"},
                });
        return mock.Object;
    }

}