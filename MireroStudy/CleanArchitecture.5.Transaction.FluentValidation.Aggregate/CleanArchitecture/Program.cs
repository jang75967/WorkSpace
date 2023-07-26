using CleanArchitecture.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddGrpc();
builder.Services.AddMapper();
builder.Services.AddRepositories();;
builder.Services.AddEFCore(configuration);
builder.Services.AddMediatR();
builder.Services.AddValidator();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.AddControllers();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }