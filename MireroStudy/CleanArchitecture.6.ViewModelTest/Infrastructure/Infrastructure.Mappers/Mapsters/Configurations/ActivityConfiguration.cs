using EntityActivity = Domain.Entities.ActivityAggregate.Activity;
using DtoActivity = Api.Activities.Activity;
using EntityExpense = Domain.Entities.ActivityAggregate.Expense;
using DtoExpense = Api.Activities.Expense;
using EntityAttendant = Domain.Entities.ActivityAggregate.Attendant;
using DtoAttendant = Api.Activities.Attendant;
using Mapster;

namespace Infrastructure.Mappers.Mapsters.Configurations;

public static class ActivityConfiguration
{
    public static TypeAdapterConfig AddActivities(this TypeAdapterConfig cfg)
    {
        cfg.NewConfig<EntityExpense, DtoExpense>().PreserveReference(true);
        cfg.NewConfig<EntityAttendant, DtoAttendant>().PreserveReference(true);
        cfg.NewConfig<EntityActivity, DtoActivity>().PreserveReference(true);

        cfg.NewConfig<DtoExpense,EntityExpense>().PreserveReference(true);
        cfg.NewConfig<DtoAttendant,EntityAttendant>().PreserveReference(true);
        cfg.NewConfig<DtoActivity,EntityActivity>().PreserveReference(true);

        //cfg.NewConfig<EntityActivity, DtoActivity>()
        //    .Map(dto => dto.Expenses, entity => entity.Expenses.Adapt<IEnumerable<EntityExpense>>())
        //    .Map(dto => dto.Attendees, entity => entity.Attendees.Adapt<IEnumerable<EntityAttendant>>());

        return cfg;
    }
}
