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

    // Program.cs 의
    // var builder = WebApplication.CreateBuilder(args);
    // 지나고 나서,
    // ConfigureTestServices 로 액션 델리게이트에 서비스 등록 후 ??????
    // Progam.cs 의
    // builder.Services.AddMapper(); 로 
    // AutoMapper 추가하고,
    // 처음 받은 소스의 services.AddTransient<IUserService, UserService>(); 를 확장메서드로 분리해서 DI 적용한거
    // Service 추가하고,
    // 쭉쭉쭉.. 일련의 과정을 거치고 나서
    // Program.cs 의
    // var app = builder.Build();
    // 실행되기 전에,
    // 추가된 서비스인 AddMapster 실행해서
    // Mapster 추가하고,
    // 의 동작을 "비동기" 로 실행
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 액션 델리게이트에 서비스 등록 (AddMapMaster)
        builder.ConfigureTestServices(services =>
        {
            services.AddMapster();
        });
    }

    // 설명에 나와있듯이, "클래스를 만든 직후 사용하기 전 호출" (Called immediately after the class has been created, before it is used.)
    // 디버깅 제일 처음, TestFactory<Program> 이라서 Program.cs 실행되면 TestFactory 생성자 지난 후,
    // IAsyncLifetime 를 구현했기 때문에, InitializeAsync 실행 (비동기)
    public Task InitializeAsync()
    {
        // 수명의 범위를 제어하는 IServiceScope (DI) : 직접 만든 범위에서 종속성을 해결하는 데 사용
        // ServiceProvider.GetService() 일정 범위 내에서는 항상 같은 인스턴스를 반환합니다.
        using var scope = Services.CreateScope();
        Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
    }
}