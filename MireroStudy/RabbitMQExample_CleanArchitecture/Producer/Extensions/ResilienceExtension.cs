using Domain.Resilience;

namespace Producer.Extensions;

public static class ResilienceExtension
{
    public static IServiceCollection AddResilience(this IServiceCollection services, IConfiguration configuration)
    {
        var retryOption = new RetryOption(Convert.ToInt32(configuration.GetSection("RETRY_COUNT").Value), 10 * 1000);
        services.AddSingleton(retryOption);
        return services;
    }
}
