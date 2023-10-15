using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Common;

public static class MockDbSet
{
    public static DbSet<TEntity> Build<TEntity>(this Mock<DbSet<TEntity>> mock) where TEntity : BaseEntity
     => mock.Object;


    public static Mock<DbSet<TEntity>> FindAsync<TEntity>(this Mock<DbSet<TEntity>> mock, List<TEntity> seed) where TEntity : BaseEntity
    {
        mock.Setup(m => m.FindAsync(It.IsAny<object[]>(), default))
           .ReturnsAsync((object[] r, CancellationToken c) => seed.ToList().Find(x => x.Id == (long)r[0]));
        return mock;
    }

    public static Mock<DbSet<TEntity>> AddAsync<TEntity>(this Mock<DbSet<TEntity>> mock, List<TEntity> seed) where TEntity : BaseEntity
    {
        mock.Setup(set => set.AddAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
          .Callback((TEntity entity, CancellationToken _) => seed.Add(entity))
          .ReturnsAsync((TEntity entity, CancellationToken c) =>
          {
              var internalEntity = MockInternalEntityEntry.GetInternalEntityEntry<TEntity>(entity);
              return new EntityEntry<TEntity>(internalEntity);
          });
        return mock;
    }

    public static Mock<DbSet<TEntity>> Update<TEntity>(this Mock<DbSet<TEntity>> mock, List<TEntity> seed) where TEntity : BaseEntity
    {
        mock.Setup(set => set.Update(It.IsAny<TEntity>()))
          .Callback((TEntity entity) =>
          {
              var target = seed.Find(x => x.Id == entity.Id);
              target = entity;
          })
          .Returns((TEntity entity) =>
          {
              var internalEntity = MockInternalEntityEntry.GetInternalEntityEntry<TEntity>(entity);
              return new EntityEntry<TEntity>(internalEntity);
          });
        return mock;
    }

    public static Mock<DbSet<TEntity>> Remove<TEntity>(this Mock<DbSet<TEntity>> mock, Action<TEntity> callback) where TEntity : BaseEntity
    {
        mock.Setup(set => set.Remove(It.IsAny<TEntity>()))
          .Callback(callback);
        return mock;
    }
}
