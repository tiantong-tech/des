using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class StockRepository : Repository<Stock>
  {
    private OrderItemRepository _orderItems;

    public StockRepository(
      DbContext db,
      OrderItemRepository orderItems
    ) : base(db) {
      _orderItems = orderItems;
    }

    public bool HasGood(int goodId)
    {
      return Table.Any(stock => stock.good_id == goodId);
    }

    public Stock[] GetByGoods(Good[] goods)
    {
      var ids = goods.SelectMany(good => good.stock_ids);

      return Table.Where(stock => ids.Contains(stock.id)).ToArray();
    }

    public Stock GetOrAdd(int warehouseId, int itemId, int locationId)
    {
      var stock = Table.Where(sk =>
        sk.item_id == itemId &&
        sk.location_id == locationId &&
        sk.warehouse_id == warehouseId
      ).FirstOrDefault();

      if (stock == null) {
        stock = new Stock {
          item_id = itemId,
          location_id = locationId,
          warehouse_id = warehouseId
        };
        Add(stock);
      }

      return stock;
    }
  }
}
