using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ItemRepository : Repository<Item, int>
  {
    private WarehouseRepository _warehouses;

    public ItemRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public Item[] GetByGoods(Good[] goods)
    {
      var ids = goods.SelectMany(good => good.item_ids).Distinct();

      return Table.Where(item => ids.Contains(item.id)).ToArray();
    }
  }
}
