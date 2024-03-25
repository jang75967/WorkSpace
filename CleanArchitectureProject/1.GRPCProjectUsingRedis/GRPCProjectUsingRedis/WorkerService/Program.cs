using Application;
using GRPCProejctUsingRedis;
using InfraStructrue.Data.Repositories;
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

            // grpc ���� ���
            builder.Services.AddGrpc();
            // MediatR ���
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
            // Repository ���
            builder.Services.AddSingleton<IFakeRepository, FakeRepsitory>();
            // UserService ���
            builder.Services.AddSingleton<IUserService, UserService>();
            // RedisManagerService ���
            builder.Services.AddSingleton<RedisManagerService>();

            // ��׶��� ���� ���
            builder.Services.AddHostedService<Worker>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserController>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}