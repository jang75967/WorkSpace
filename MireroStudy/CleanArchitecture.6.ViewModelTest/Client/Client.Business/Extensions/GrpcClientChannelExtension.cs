using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client.Business.Extensions;

public static class GrpcClientChannelExtension
{
    public static IServiceCollection AddGrpcClientChannel(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(
           GrpcChannel.ForAddress(configuration.GetSection("API_SERVICE_URL").Value!,
           new GrpcChannelOptions()
           {
               LoggerFactory = LoggerFactory.Create(logger => logger.SetMinimumLevel(LogLevel.Debug)),
               MaxReceiveMessageSize = 1566 * 1024 * 1024, // 1.5GB
               MaxSendMessageSize = 1566 * 1024 * 1024, // 1.5GB
           }
           ));
        return services;
    }
}
