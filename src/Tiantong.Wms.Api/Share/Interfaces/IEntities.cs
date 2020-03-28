using System.Collections.Generic;

namespace Tiantong.Wms.Api
{
  public interface IEntities<TEntity, TKey> where TEntity : IEntity<TKey>
  {
    TKey[] Keys { get; }

    Dictionary<string, TEntity> Data { get; }

    object Relationships { get; set; }
  }

}
