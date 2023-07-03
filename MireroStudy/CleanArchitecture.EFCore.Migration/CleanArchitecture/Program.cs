using CleanArchitecture.Extensions;
using CleanArchitecture.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddGrpc();
builder.Services.AddMapper();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddEFCore(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//await app.Migration();
app.MapGrpcService<UserController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }