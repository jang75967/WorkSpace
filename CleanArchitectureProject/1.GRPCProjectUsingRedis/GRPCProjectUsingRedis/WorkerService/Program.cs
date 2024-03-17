using Application;
using Microsoft.AspNetCore.Builder;
using WorkerService.Controllers;
using WorkerService.Services;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = Host.CreateApplicationBuilder(args);
            //builder.Services.AddHostedService<Worker>();

            //var host = builder.Build();
            //host.Run();



            var builder = WebApplication.CreateBuilder(args);

            // grpc ���� ���
            builder.Services.AddGrpc();
            builder.Services.AddSingleton<IUserService, UsersService>();

            // ��׶��� ���� ���
            //builder.Services.AddHostedService<Worker>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserController>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}