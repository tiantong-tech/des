using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WebRoutes : RouterProvider
  {
    public override void Define()
    {
      Get("/", "App.Home");
      Post("/", "App.Home");

      Post("/dev/initialize", "App.Initialize");
      Post("/dev/restore", "App.Restore");
      Post("/dev/seed", "App.InsertTestData");
      Post("/dev/migrate", "App.Migrate");
      Post("/dev/rollback", "App.Rollback");
      Post("/dev/refresh", "App.Refresh");
      Post("/dev/reseed", "App.Reseed");

      Post("/users/search", "User.Search");

      Post("/users/register", "User.Register");
      Post("/users/person/update", "User.UpdatePerson");
      Post("/users/person/profile", "User.GetPersonProfile");

      Post("/auth/email", "User.AuthByEmail");
      Post("/auth/token/refresh", "User.RefreshToken");

      Post("/warehouses/create", "Warehouse.Create");
      Post("/warehouses/delete", "Warehouse.Delete");
      Post("/warehouses/update", "Warehouse.Update");
      Post("/warehouses/search", "Warehouse.Search");
      Post("/warehouses/find", "Warehouse.Find");

      Post("/areas/create", "Area.Create");
      Post("/areas/delete", "Area.Delete");
      Post("/areas/update", "Area.Update");
      Post("/areas/search", "Area.Search");

      Post("/locations/create", "Location.Create");
      Post("/locations/delete", "Location.Delete");
      Post("/locations/update", "Location.Update");
      Post("/locations/search", "Location.Search");

      Post("/projects/create", "Project.Create");
      Post("/projects/delete", "Project.Delete");
      Post("/projects/update", "Project.Update");
      Post("/projects/search", "Project.Search");
      Post("/projects/find", "Project.Find");

      Post("/good-categories/create", "GoodCategory.Create");
      Post("/good-categories/delete", "GoodCategory.Delete");
      Post("/good-categories/update", "GoodCategory.Update");
      Post("/good-categories/search", "GoodCategory.Search");
      Post("/good-categories/find", "GoodCategory.Find");

      Post("/orders/categories/create", "OrderCategory.Create");
      Post("/orders/categories/delete", "OrderCategory.Delete");
      Post("/orders/categories/update", "OrderCategory.Update");
      Post("/orders/categories/search", "OrderCategory.Search");

      Post("/goods/create", "Good.Create");
      Post("/goods/delete", "Good.Delete");
      Post("/goods/update", "Good.Update");
      Post("/goods/search", "Good.Search");
      Post("/goods/find", "Good.Find");

      Post("/suppliers/create", "Supplier.Create");
      Post("/suppliers/delete", "Supplier.Delete");
      Post("/suppliers/update", "Supplier.Update");
      Post("/suppliers/search", "Supplier.Search");
      Post("/suppliers/find", "Supplier.Find");

      Post("/suppliers/create", "Supplier.Create");
      Post("/suppliers/delete", "Supplier.Delete");
      Post("/suppliers/update", "Supplier.Update");
      Post("/suppliers/search", "Supplier.Search");

      Post("/orders/search", "OrderInboundComplete.Search");
      Post("/orders/complete", "OrderInboundComplete.Handle");

      // Post("/orders/inbounds/create", "OrderInbound.Create");
      // Post("/orders/inbounds/search", "OrderInbound.Search");
      // Post("/orders/inbounds/submit", "OrderInbound.Submit");

      // Post("/orders/outbounds/create", "Order.OutboundCreate");
      // Post("/orders/outbounds/search", "Order.OutboundSearch");
      // Post("/orders/outbounds/submit", "Order.OutboundSubmit");

      // Post("/orders/inventories/create", "Order.InventoryCreate");
      // Post("/orders/inventories/search", "Order.InventorySearch");
      // Post("/orders/inventories/submit", "Order.InventorySubmit");

      // Post("/orders/returns/create", "Order.ReturnCreate");
      // Post("/orders/returns/search", "Order.ReturnSearch");
      // Post("/orders/returns/submit", "Order.ReturnSubmit");

      // Post("/stocks/search", "Stock.Search");
      // Post("/stocks/records/search", "StockRecord.Search");

    }
  }
}
