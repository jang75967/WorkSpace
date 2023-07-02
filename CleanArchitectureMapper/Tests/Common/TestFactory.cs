using Application.Mappers;
using Grpc.Net.Client;
using Infrastructure.Mappers.Mapsters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Common;

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

    // Program.cs ��
    // var builder = WebApplication.CreateBuilder(args);
    // ������ ����,
    // ConfigureTestServices �� �׼� ��������Ʈ�� ���� ��� �� ??????
    // Progam.cs ��
    // builder.Services.AddMapper(); �� 
    // AutoMapper �߰��ϰ�,
    // ó�� ���� �ҽ��� services.AddTransient<IUserService, UserService>(); �� Ȯ��޼���� �и��ؼ� DI �����Ѱ�
    // Service �߰��ϰ�,
    // ������.. �Ϸ��� ������ ��ġ�� ����
    // Program.cs ��
    // var app = builder.Build();
    // ����Ǳ� ����,
    // �߰��� ������ AddMapster �����ؼ�
    // Mapster �߰��ϰ�,
    // �� ������ "�񵿱�" �� ����
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // �׼� ��������Ʈ�� ���� ��� (AddMapMaster)
        builder.ConfigureTestServices(services =>
        {
            services.AddMapster();
        });
    }

    // ���� �����ֵ���, "Ŭ������ ���� ���� ����ϱ� �� ȣ��" (Called immediately after the class has been created, before it is used.)
    // ����� ���� ó��, TestFactory<Program> �̶� Program.cs ����Ǹ� TestFactory ������ ���� ��,
    // IAsyncLifetime �� �����߱� ������, InitializeAsync ���� (�񵿱�)
    public Task InitializeAsync()
    {
        // ������ ������ �����ϴ� IServiceScope (DI) : ���� ���� �������� ���Ӽ��� �ذ��ϴ� �� ���
        // ServiceProvider.GetService() ���� ���� �������� �׻� ���� �ν��Ͻ��� ��ȯ�մϴ�.
        using var scope = Services.CreateScope();
        Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
    }
}