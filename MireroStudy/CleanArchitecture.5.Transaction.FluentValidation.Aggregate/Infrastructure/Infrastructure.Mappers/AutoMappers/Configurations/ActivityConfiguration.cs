using AutoMapper;
using EntityActivity = Domain.Entities.ActivityAggregate.Activity;
using DtoActivity = Api.Activities.Activity;
using EntityExpense = Domain.Entities.ActivityAggregate.Expense;
using DtoExpense = Api.Activities.Expense;
using EntityAttendant = Domain.Entities.ActivityAggregate.Attendant;
using DtoAttendant = Api.Activities.Attendant;

namespace Infrastructure.Mappers.AutoMappers.Configurations;

public static class ActivityConfiguration
{
    public static IMapperConfigurationExpression AddActivities(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<EntityActivity, DtoActivity>().ReverseMap();
        cfg.CreateMap<EntityExpense, DtoExpense>().ReverseMap();
        cfg.CreateMap<EntityAttendant, DtoAttendant>().ReverseMap();
        return cfg;
    }
}
