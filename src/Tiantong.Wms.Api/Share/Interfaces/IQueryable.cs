using System.Linq;
using System.Collections.Generic;

namespace Tiantong.Wms.Api
{
  public static class IQueryableExtensions
  {
    /// 将 array 映射为 data (dictionary), 以 GetStringKey 作为 key
    public static Dictionary<string, TEntity> ToRelationship<TEntity, TKey>(this IQueryable<TEntity> query) where TEntity: IEntity<TKey>
    {
      var data = new Dictionary<string, TEntity>();

      foreach (var entity in query.ToArray()) {
        data.Add(entity.GetStringKey(), entity);
      }

      return data;
    }

    public static Dictionary<string, TEntity> ToRelationship<TEntity>(this IQueryable<TEntity> query) where TEntity: IEntity<int>
    {
      return query.ToRelationship<TEntity, int>();
    }

    public static Dictionary<TKey, TEntity> ToDictionary<TEntity, TKey>(this IQueryable<TEntity> query) where TEntity: IEntity<TKey>
    {
      var data = new Dictionary<TKey, TEntity>();

      foreach (var entity in query.ToArray()) {
        data.Add(entity.GetKey(), entity);
      }

      return data;
    }

    // 将 array 映射为 keys(array) + data, array 类型为 TKey[]
    public static IEntities<TEntity, TKey> ToEntities<TEntity, TKey>(this IQueryable<TEntity> query) where TEntity : IEntity<TKey>
    {
      var keys = new List<TKey>();
      var data = new Dictionary<string, TEntity>();
      var entities = new Entities<TEntity, TKey>();

      foreach (var entity in query.ToArray()) {
        keys.Add(entity.GetKey());
        data.Add(entity.GetStringKey(), entity);
      }

      entities.Keys = keys.ToArray();
      entities.Data = data;

      return entities;
    }

    public static IEntities<TEntity, int> ToEntities<TEntity>(this IQueryable<TEntity> query) where TEntity: IEntity<int>
    {
      return query.ToEntities<TEntity, int>();
    }

    public static IPagination<TEntity, TKey> Paginate<TEntity, TKey>(this IQueryable<TEntity> query, int page, int pageSize) where TEntity: IEntity<TKey>
    {
      var total = query.Count();
      var entities = query.ToEntities<TEntity, TKey>();

      return new Pagination<TEntity, TKey> {
        Keys = entities.Keys,
        Data = entities.Data,
        Meta = new Meta(page, pageSize, total),
      };
    }

    public static IPagination<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, int page, int pageSize) where TEntity: IEntity<int>
    {
      var pagination = query.Paginate<TEntity, int>(page, pageSize);

      return new Pagination<TEntity> {
        Meta = pagination.Meta,
        Keys = pagination.Keys,
        Data = pagination.Data,
      };
    }
  }
}
