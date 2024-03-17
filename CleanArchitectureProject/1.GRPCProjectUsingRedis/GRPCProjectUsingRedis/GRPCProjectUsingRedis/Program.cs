using Application;
using GRPCProejctUsingRedis.Services;

namespace GRPCProejctUsingRedis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // grpc ���� ���
            builder.Services.AddGrpc();
            // MediatR ���
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
            // UserService ���
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