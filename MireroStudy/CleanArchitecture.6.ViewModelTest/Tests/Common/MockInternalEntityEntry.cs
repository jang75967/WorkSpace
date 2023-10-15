using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;

namespace Common;

public static class MockInternalEntityEntry
{
    public static InternalEntityEntry GetInternalEntityEntry<T>(T entity) where T : BaseEntity
    {
        return new InternalEntityEntry(
        new Mock<IStateManager>().Object,
        new RuntimeEntityType(
            name: nameof(T), type: typeof(T), sharedClrType: false, model: new(),
            baseType: null, discriminatorProperty: null, changeTrackingStrategy: ChangeTrackingStrategy.Snapshot,
            indexerPropertyInfo: null, propertyBag: false,
            discriminatorValue: null),
        entity);
    }
}
