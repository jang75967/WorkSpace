﻿using Application.Persistences;
using InfraStructrue.Data.Persistence.MessageBus.Redis;
using InfraStructure.Data.Persistence.EFCore;
using InfraStructure.Data.Persistence.MessageBus.RabbitMQ;

namespace WorkerService.Extensions
{
    public static class PersistenceExtension
    {
        public static IServiceCollection AddQueue(this IServiceCollection services)
        {
            //services.AddSingleton<IQueue, RedisQueue>();
            services.AddSingleton<IQueue, RabbitMQQueue>();

            return services;
        }

        public static IServiceCollection AddEFCore(this IServiceCollection services)
        {
            services.AddPostgresSql();
            
            return services;
        }
    }
}
