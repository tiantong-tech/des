using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renet.Utils;
using Renet.Web;
using System;
using System.Linq;

namespace Tiantong.Wms.Api
{
  public class DevelopmentController : BaseController
  {
    private Auth _auth;

    private Config _config;

    private DbContext _db;

    private IRandom _random;

    private UserRepository _users;

    private RootRepository _rootRepository;

    private WarehouseRepository _warehouses;

    private MigratorProvider _migratorProvider;

    public DevelopmentController(
      Auth auth,
      DbContext db,
      Config config,
      IRandom random,
      UserRepository users,
      RootRepository rootRepository,
      WarehouseRepository warehouses,
      MigratorProvider migratorProvider
    ) {
      _db = db;
      _migratorProvider = migratorProvider;
      _auth = auth;
      _config = config;
      _random = random;
      _users = users;
      _warehouses = warehouses;
      _rootRepository = rootRepository;

      if (!_config.IsDevelopment) {
        throw new FailureOperation("此接口只适用于开发环境");
      }

    }

    [HttpPost]
    [Route("/dev/migrate")]
    public object Migrate()
    {
      _migratorProvider.Migrator.Migrate();

      return SuccessOperation("数据库已建立");
    }

    [HttpPost]
    [Route("/dev/rollback")]
    public object Rollback()
    {
      _migratorProvider.Migrator.Rollback();

      return SuccessOperation("数据库已回档");
    }

    [HttpPost]
    [Route("/dev/refresh")]
    public object refresh()
    {
      _migratorProvider.Migrator.Refresh();

      return SuccessOperation("数据库已重建");
    }

    [HttpPost]
    [Route("/dev/seed")]
    public object Seed()
    {
      InsertRootUser();
      InsertOwnerUsers();
      InsertWarehouses();
      InsertWarehouseUsers();
      InsertDepartments();
      InsertAreas();
      InsertLocations();
      InsertProjects();
      InsertSuppliers();
      InsertGoodCategories();
      InsertGoods();
      InsertStocks();
      InsertOrders();

      return SuccessOperation("测试数据建立完毕");
    }

    [HttpPost]
    [Route("/dev/reseed")]
    public object Reseed()
    {
      _migratorProvider.Migrator.Refresh();

      return Seed();
    }

    private void InsertRootUser()
    {
      _rootRepository.Create();
    }

    private void InsertOwnerUsers()
    {
      _random.For(2, 2)(i => {
        _users.Add(new User {
          type = UserType.User,
          password = "123456",
          name = $"测试用户_{i}",
          email = "owner" + (i == 1 ? "" : i.ToString()) + "@wms.com"
        });
      });

      _users.UnitOfWork.SaveChanges();
    }

    private void InsertWarehouses()
    {
      _users.Owners.OrderBy(user => user.id).Take(3).ToList().ForEach(owner => {
        _random.For(2, 2)(i => {
          _warehouses.Add(new Warehouse() {
            number = $"WH000{i}",
            name = $"测试仓库 {i}",
            address = $"地址 {i}",
            comment = $"备注 {i}",
          }, owner.id);
        });
      });
    }

    private void InsertDepartments()
    {
      _db.Warehouses.ToList().ForEach(warehouse => {
        _random.For(2, 10)(i => {
          _db.Departments.Add(new Department {
            warehouse_id = warehouse.id,
            name = $"测试部门{i}",
            comment = $"测试部门备注_{i}",
            type = DepartmentType.Custom
          });
        });
      });

      _db.SaveChanges();
    }

    private void InsertWarehouseUsers()
    {
      foreach (var warehouse in _db.Warehouses.ToArray()) {
        var departments = _db.Departments.Where(w => w.warehouse_id == warehouse.id).ToArray();
        foreach (var i in _random.Enumerate(20, 30)) {
          _db.WarehouseUsers.Add(new WarehouseUser {
            warehouse_id = warehouse.id,
            department_id = _random.Array(departments).id,
            user = new User {
              type = UserType.User,
              name = $"用户_{i}",
              password = "aeoikj",
              email = $"warehouse{warehouse.id}_user{i}@wms.com"
            }
          });
        }
      }

      _db.SaveChanges();
    }

    private void InsertAreas()
    {
      _db.Warehouses.OrderBy(warehouse => warehouse.id).ToList().ForEach(warehouse => {
        _random.For(2, 5)(i => {
          _db.Areas.Add(new Area() {
            warehouse_id = warehouse.id,
            number = $"AR{i}",
            name = $"测试区域 {i}",
            comment = $"测试区域备注 {i}",
            total_area = $"{_random.Int(100, 500)}",
          });
        });
      });

      _db.SaveChanges();
    }

    private void InsertLocations()
    {
      _db.Areas.OrderBy(item => item.id).ToList().ForEach(area => {
        _random.For(2, 5)(i => {
          _db.Locations.Add(new Location() {
            area_id = area.id,
            total_area = $"{i}",
            total_volume = $"{i * i}",
            warehouse_id = area.warehouse_id,
            name = $"测试区域 {i}",
            number = $"LN{area.id}.{i}",
            comment = $"测试区域备注 {i}",
          });
        });
      });

      _db.SaveChanges();
    }

    private void InsertProjects()
    {
      foreach (var warehouse in _db.Warehouses.ToArray()) {
        _random.For(20, 50)(i => {
          var now = DateTime.Now;
          var startAt = _random.DateTime(now.AddDays(-30), now.AddDays(30));
          var dueTime = _random.DateTime(startAt.AddDays(30), startAt.AddDays(180));
          var finishedAt = _random.Bool() ? _random.DateTime(dueTime.AddDays(-60), dueTime.AddDays(60)) : DateTime.MinValue;

          _db.Projects.Add(new Project {
            warehouse_id = warehouse.id,
            number = $"PJ{i}",
            name = $"测试项目 {i}",
            comment = $"测试项目备注 {i}",
            due_time = dueTime,
            started_at = startAt,
            finished_at = finishedAt,
          });
        });
      }

      _db.SaveChanges();
    }

    private void InsertSuppliers()
    {
      foreach (var warehouse in _db.Warehouses.ToArray()) {
        for (int i = 0, L = _random.Int(20, 50); i < L; i++) {
          _db.Suppliers.Add(new Supplier {
            warehouse_id = warehouse.id,
            name = $"测试供应商 {i}",
            comment = $"测试供应商备注 {i}",
          });
        }
      }

      _db.SaveChanges();
    }

    private void InsertGoodCategories()
    {
      foreach (var warehouse in _db.Warehouses.ToArray()) {
        _random.For(2, 5)(i => {
          _db.GoodCategories.Add(new GoodCategory {
            warehouse_id = warehouse.id,
            name = $"测试分类 {i}",
            comment = $"测试分类备注 {i}",
          });
        });
      }

      _db.SaveChanges();
    }

    private void InsertGoods()
    {
      foreach (var warehouse in _db.Warehouses.ToArray()) {
        _random.For(20, 40)(i => {
          _db.Goods.Add(new Good {
            warehouse_id = warehouse.id,
            number = $"item_000{i}",
            name = $"测试物品 {i}",
            comment = $"测试物品备注 {i}",
            items = _random.Enumerate(1, 5).Select(j => new Item {
              warehouse_id = warehouse.id,
              index = j,
              name = $"测试规格 {j}",
              number = $"item_{i}_{j}",
              unit = "个",
            }).ToList()
          });
        });
      }

      _db.SaveChanges();
    }

    private void InsertStocks()
    {
      foreach (var warehouse in _db.Warehouses.ToArray()) {
        var locations = _db.Locations.Where(location => location.warehouse_id == warehouse.id).ToArray();
        var goods = _db.Goods.Include(good => good.items).Where(good => good.warehouse_id == warehouse.id).ToList();

        goods.ForEach(good => good.items.ForEach(item => {
          foreach (var location in _random.Array(locations, 2, 5)) {
            _db.Stocks.Add(new Stock {
              warehouse_id = good.warehouse_id,
              good_id = item.good_id,
              item_id = item.id,
              area_id = location.area_id,
              location_id = location.id,
              quantity = _random.Int(10, 100),
              records = _random.Enumerate(1, 10).Select(i => new StockRecord {
                order_id = _random.Int(0, 10),
                quantity = _random.Int(1, 10),
              }).ToList()
            });
          }
        }));
      }

      _db.SaveChanges();
    }

    public void InsertOrders()
    {
      foreach (var warehouse in _db.Warehouses.ToArray()) {
        var projects = _db.Projects.Where(pj => pj.warehouse_id == warehouse.id).ToArray();
        var suppliers = _db.Suppliers.Where(sp => sp.warehouse_id == warehouse.id).ToArray();
        var departments = _db.Departments.Where(dp => dp.warehouse_id == warehouse.id).ToArray();
        var users = _db.WarehouseUsers.Include(wu => wu.user).Where(wu => wu.warehouse_id == warehouse.id).ToArray();
        var goods = _db.Goods.Include(good => good.items).Where(good => good.warehouse_id == warehouse.id).ToArray();

        foreach (var i in _random.Enumerate(20, 40)) {
          _db.Orders.Add(new Order {
            warehouse_id = warehouse.id,
            number = $"PR{i}",
            status = _random.Bool() ? OrderStatus.Created : OrderStatus.Finished,
            comment = "测试订单",
            type = _random.Bool() ? OrderType.PurchaseRequisition : OrderType.Requisition,
            operator_id = _random.Array(users).id,
            applicant_id = _random.Array(users).user_id,
            department_id = _random.Array(departments).id,
            supplier_id = _random.Array(suppliers).id,
            items = _random.Enumerate(1, 10).Select(j => {
              var item = _random.Array(_random.Array(goods).items.ToArray());

              return new OrderItem {
                index = j,
                item_id = item.id,
                good_id = item.good_id,
                price = _random.Int(1000, 100000),
                quantity = _random.Int(1, 100),
                comment = $"测试物品备注_{j}",
                delivery_cycle = _random.Int(30, 90).ToString() + '天',

                invoice = new Invoice {
                  name = $"测试物品_{j}",
                  specification = $"测试规格_{j}",
                  unit = "个",
                  quantity = 12,
                  price = _random.Int(10000, 100000),
                  amount = _random.Int(10000, 100000),
                  tax_rate = _random.Int(1, 20),
                  tax_amount = _random.Int(1000, 10000),
                  type = "增值税专用发票",
                  number = _random.Int(100000009, 999999999).ToString(),
                },
                projects = _random.Array(projects, _random.Int(0, 3)).Select(
                  (project, index) => new OrderItemProject {
                    index = index,
                    project_id = project.id,
                    quantity = _random.Int(1, 10),
                  }
                ).ToList(),
              };
            }).ToList(),
            payments = _random.Enumerate(1, 3).Select(j => new OrderPayment {
              index = j,
              is_paid = _random.Bool(),
              amount = _random.Int(10000, 100000),
              comment = $"付款_{j}",
              due_time = DateTime.Now.AddDays(_random.Int(10, 30)),
              paid_at = DateTime.Now.AddDays(_random.Int(0, 30)),
            }).ToList()
          });

          _db.SaveChanges();
        }
      }

    }

  }
}