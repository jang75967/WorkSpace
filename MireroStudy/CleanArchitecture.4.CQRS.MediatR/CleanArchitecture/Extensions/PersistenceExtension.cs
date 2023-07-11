﻿using Infrastructure.EFCore;

namespace CleanArchitecture.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
    {
       // services.AddTransient<IApplicationDbContext, Infrastructure.EFCore.ApplicationDbContext>();
        services.AddPostgreSql(configuration);
        //services.AddOracle(configuration);
        //services.AddMsSql(configuration);
        //services.AddMongoDB(configuration);
        return services;
    }
}
