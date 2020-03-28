using System.Collections.Generic;

namespace Tiantong.Wms.Api
{
  public class Entities<TEntity, TKey>: IEntities<TEntity, TKey> where TEntity: IEntity<TKey>
  {
    public TKey[] Keys { get; set; }

    public Dictionary<string, TEntity> Data { get; set; }

    public object Relationships { get; set; }
  }

  public class Entities<TEntity>: Entities<TEntity, int> where TEntity: IEntity<int>
  {

  }

}
