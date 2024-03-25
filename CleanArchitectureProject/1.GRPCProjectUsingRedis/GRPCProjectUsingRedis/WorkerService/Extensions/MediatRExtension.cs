namespace WorkerService.Extensions
{
    public static class MediatRExtension
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TranscationBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
