using Microsoft.AspNetCore.Builder;
using WorkerService.Extensions;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();
            builder.Services.AddMapper();
            builder.Services.AddRepositories();
            builder.Services.AddMessageBoxOptionConfigure(builder.Configuration);
            builder.Services.AddMessageBusOptionSetup();
            builder.Services.AddQueue();
            builder.Services.AddMediatR();
            builder.Services.AddWorkerService();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.AddControllers();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}