using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class GoodController : BaseController
  {
    private IAuth _auth;

    private GoodRepository _goods;

    private StockRepository _stocks;

    private LocationRepository _locations;

    private OrderItemRepository _orderItems;

    private WarehouseRepository _warehouses;

    private GoodCategoryRepository _goodCategories;

    public GoodController(
      IAuth auth,
      GoodRepository goods,
      StockRepository stocks,
      LocationRepository locations,
      WarehouseRepository warehouses,
      GoodCategoryRepository itemCategories
    ) {
      _auth = auth;
      _goods = goods;
      _stocks = stocks;
      _locations = locations;
      // _orderItems = orderItems;
      _warehouses = warehouses;
      _goodCategories = itemCategories;
    }

    public class CreateParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      public int[] category_ids { get; set; } = new int[] {};

      public string number { get; set; }

      [Required]
      public string name { get; set; }

      public string comment { get; set; } = "";

    }

    public object Create([FromBody] CreateParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      _goodCategories.EnsureIds(param.warehouse_id, param.category_ids);

      var location = _locations.Table
        .Where(ltn => ltn.warehouse_id == param.warehouse_id)
        .First();
      var category = _goodCategories.Table
        .Where(ctg => ctg.warehouse_id == param.warehouse_id)
        .First();

      param.category_ids = new int[] { category.id };

      if (param.number != null) {
        _goods.EnsureNumberUnique(param.warehouse_id, param.number);
      }

      var item = new Good {
        warehouse_id = param.warehouse_id,
        number = param.number,
        category_ids = param.category_ids,
        name = param.name,
        comment = param.comment,
      };
      _goods.Add(item);

      _goods.UnitOfWork.BeginTransaction();

      _goods.UnitOfWork.SaveChanges();
      _stocks.Add(new Stock {
        warehouse_id = param.warehouse_id,
        item_id = item.id,
        location_id = location.id
      });
      _goods.UnitOfWork.SaveChanges();
      _goods.UnitOfWork.Commit();

      return SuccessOperation("货品已创建", item.id);
    }

    public class DeleteParams
    {
      [Nonzero]
      public int id { get; set; }
    }

    public object Delete([FromBody] DeleteParams param)
    {
      _auth.EnsureOwner();
      var item = _goods.EnsureGetByOwner(param.id, _auth.User.id);
      if (_stocks.HasGood(param.id)) {
        return FailureOperation("货品已被使用，无法删除");
      }
      _goods.Remove(item.id);
      _goods.UnitOfWork.SaveChanges();

      return SuccessOperation("货品已删除");
    }

    public class UpdateParams
    {
      [Nonzero]
      public int id { get; set; }

      public string number { get; set; }

      public int[] categroy_ids { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] UpdateParams param)
    {
      _auth.EnsureOwner();
      var item = _goods.EnsureGetByOwner(param.id, _auth.User.id);
      if (param.number != null) {
        _goods.EnsureNumberUnique(item.warehouse_id, param.number);
        item.number = param.number;
      }
      if (param.categroy_ids != null) {
        _goodCategories.EnsureIds(item.warehouse_id, param.categroy_ids);
        item.category_ids = param.categroy_ids;
      }
      if (param.name != null) item.name = param.name;
      if (param.comment != null) item.comment = param.comment;
      if (param.is_enabled != null) {
        item.is_enabled = (bool) param.is_enabled;
      }
      _goods.UnitOfWork.SaveChanges();

      return SuccessOperation("货品信息已保存");
    }

    public class SearchParams : BaseSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public IPagination<Good> Search([FromBody] SearchParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);

      var goods = _goods.Table
        .Where(good => good.warehouse_id == param.warehouse_id && (
          param.search == null ? true :
          good.name.Contains(param.search) ||
          good.number.Contains(param.search)
        ))
        .OrderByDescending(good => good.is_enabled)
        .ThenBy(good => good.number)
        .ThenBy(good => good.id)
        .Paginate<Good>(param.page, param.page_size);

      goods.Relationships = new {
        stocks = _stocks.GetByGoods(goods.Data.Values.ToArray())
      };

      return goods;
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    public Good Find([FromBody] FindParams param)
    {
      _auth.EnsureOwner();

      return _goods.EnsureGetByOwner(param.id, _auth.User.id);
    }
  }
}
