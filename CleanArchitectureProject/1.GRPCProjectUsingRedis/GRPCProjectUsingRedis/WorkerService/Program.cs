using Application;
using GRPCProejctUsingRedis;
using Microsoft.AspNetCore.Builder;
using WorkerService.Controllers;
using WorkerService.Services;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // grpc 서비스 등록
            builder.Services.AddGrpc();
            // MediatR 등록
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
            // UserService 등록
            builder.Services.AddSingleton<IUserService, UsersService>();
            // RedisManagerService 등록
            builder.Services.AddSingleton<RedisManagerService>();

            // 백그라운드 서비스 등록
            builder.Services.AddHostedService<Worker>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserController>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}