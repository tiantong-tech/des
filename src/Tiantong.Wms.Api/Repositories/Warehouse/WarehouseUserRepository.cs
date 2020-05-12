using System.Linq;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseUserRepository : Repository<WarehouseUser, int>
  {
    private Auth _auth;

    private UserRepository _users;

    private WarehouseRepository _warehouses;

    public WarehouseUserRepository(
      Auth auth,
      DbContext db,
      UserRepository users,
      WarehouseRepository warehouses
    ) : base(db) {
      _auth = auth;
      _users = users;
      _warehouses = warehouses;
    }

    // Create

    public WarehouseUser Invite(int warehouseId, int departmentId, string email)
    {
      var user = _users.EnsureGetByEmail(email);
      EnsureUnique(warehouseId, email);

      var wu = new WarehouseUser {
        warehouse_id = warehouseId,
        department_id = departmentId,
        user_id = user.id
      };

      DbContext.Add(wu);

      return wu;
    }

    public WarehouseUser Add(int warehouseId, int departmentId, string email, string name)
    {
      EnsureUnique(warehouseId, email);

      var user = new User {
        email = email,
        name = name,
        type = UserType.User,
        password = "123456",
      };

      _users.EncodePassword(user);

      var wu = new WarehouseUser {
        warehouse_id = warehouseId,
        department_id = departmentId,
        user = user,
      };

      DbContext.Add(wu);

      return wu;
    }

     public override WarehouseUser Add(WarehouseUser wu)
    {
      EnsureUser(wu.warehouse_id);
      if (wu.user == null) {
        EnsureUnique(wu);
        DbContext.Add(wu);
      } else if (wu.user != null) {
        var user = _users.GetByEmail(wu.user.email);

        if (user != null) {
          wu.user_id = user.id;
          EnsureUnique(wu);
          user.name = wu.user.name;
          wu.user = null;
          DbContext.Add(wu);
        } else {
          wu.user.type = UserType.User;
          wu.user.password = "123456";
          _users.EnsureUnique(wu.user);
          _users.EncodePassword(wu.user);
        }

        DbContext.Add(wu);
      } else {
        throw new FailureOperation("仓库信息错误");
      }

      return wu;
    }

    public override bool Remove(int id)
    {
      var wu = EnsureGet(id);
      EnsureUser(wu.warehouse_id);

      if (IsOwner(wu.warehouse_id, wu.user_id)) {
        throw new FailureOperation("无法删除仓库的拥有者");
      } else {
        DbContext.Remove(wu);
      }

      return true;
    }

    // Update

    public override WarehouseUser Update(WarehouseUser wu)
    {
      var oldWu = EnsureGet(wu.warehouse_id, wu.user_id);
      if (oldWu.department_id != wu.department_id) {
        if (oldWu.department.type == DepartmentType.Owner) {
          var count = Table
            .Include(wu => wu.department)
            .Count(wu => wu.department.id == oldWu.department_id);
          if (count == 1) {
            throw new FailureOperation("唯一仓库所有者不可变更");
          }
        }
      }
      EnsureUser(wu.warehouse_id);
      _users.EnsureUnique(wu.user);

      var user = wu.user;
      var oldUser = _users.EnsureGet(user.id);

      oldUser.name = user.name;
      oldUser.email = user.email;
      oldWu.department_id = wu.department_id;

      return wu;
    }

    // Select

    public bool IsOwner(int warehouseId, int userId)
    {
      return Table
        .Include(wu => wu.department)
        .Any(wu => 
          wu.user_id == userId &&
          wu.warehouse_id == warehouseId &&
          wu.department.type == DepartmentType.Owner
        );
    }

    public bool IsAdmin(int warehouseId, int userId)
    {
      return Table
        .Include(wu => wu.department)
        .Any(wu =>
          wu.user_id == userId &&
          wu.warehouse_id == warehouseId && (
            wu.department.type == DepartmentType.Owner ||
            wu.department.type == DepartmentType.Admin
          )
        );
    }

    public bool HasUser(int warehouseId, int userId)
    {
      return Table
        .Include(wu => wu.department)
        .Any(wu =>
          wu.user_id == userId &&
          wu.warehouse_id == warehouseId
        );
    }

    public void EnsureOwner(int warehouseId, int userId)
    {
      if (!IsOwner(warehouseId, userId)) {
        throw new FailureOperation("该操作需要仓库所有者权限");
      }
    }

    public void EnsureOwner(int warehouseId)
    {
      EnsureOwner(warehouseId, _auth.User.id);
    }

    public void EnsureAdmin(int warehouseId, int userId)
    {
      if (!IsAdmin(warehouseId, userId)) {
        throw new FailureOperation("该操作需要仓库管理员权限");
      }
    }

    public void EnsureAdmin(int warehouseId)
    {
      EnsureAdmin(warehouseId, _auth.User.id);
    }

    public WarehouseUser Find(int id)
    {
      var wu = EnsureGet(id);
      EnsureUser(wu.warehouse_id);

      return wu;
    }

    public class PersonParams
    {
      public int warehouse_id { get; set; }
    }

    public WarehouseUser Person(int warehouseId)
    {
      var userId = _auth.User.id;
      var wu = Table
        .Include(wu => wu.user)
        .Include(wu => wu.department)
        .FirstOrDefault(wu => wu.warehouse_id == warehouseId && wu.user_id == userId);

      if (wu == null) {
        throw new FailureOperation("该用户不存在");
      }

      return wu;
    }

    public IEntities<WarehouseUser, int> SearchAll(int warehouseId, string search)
    {
      return Table
        .Include(wu => wu.user)
        .Include(wu => wu.department)
        .OrderBy(wu => wu.id)
        .Where(wu =>
          wu.warehouse_id == warehouseId &&
          (
            search == null ? true :
            wu.user.name.Contains(search) ||
            wu.user.email.Contains(search)
          )
        )
        .OrderBy(wu => wu.user.name)
        .ToEntities();
    }

    public void EnsureUser(int warehouseId)
    {
      if (
        !Table.Any(wu =>
          wu.user_id == _auth.User.id &&
          wu.warehouse_id == warehouseId
        )
      ) {
        throw new FailureOperation("仓库用户认证失败");
      }
    }

    public void EnsureExists(int warehouseId, int userId)
    {
      if (
        !Table.Any(wu =>
          wu.user_id == userId &&
          wu.warehouse_id == warehouseId
        )
      ) {
        throw new FailureOperation("用户不存在");
      }
    }

    public void EnsureUnique(int warehouseId, string email)
    {
      if (
        Table.Include(wu => wu.user).Any(wu =>
          wu.warehouse_id == warehouseId &&
          wu.user.email == email
        )
      ) {
        throw new FailureOperation("该用户已存在");
      }
    }

    public void EnsureUnique(WarehouseUser wu)
    {
      if (
        Table.Any(w =>
          w.warehouse_id == wu.warehouse_id &&
          w.user_id == wu.user_id
        )
      ) {
        throw new FailureOperation("该用户已存在");
      }
    }

    public WarehouseUser EnsureGet(int id)
    {
      var wu = Table
        .Include(wu => wu.user)
        .Include(wu => wu.department)
        .Where(wu => wu.id == id)
        .SingleOrDefault();

      if (wu == null) {
        throw new FailureOperation("用户不存在");
      }

      return wu;
    }

    public WarehouseUser EnsureGet(int warehouseId, int userId)
    {
      var wu = Table
        .Include(wu => wu.user)
        .Include(wu => wu.department)
        .FirstOrDefault(wu =>
          wu.user_id == userId &&
          wu.warehouse_id == warehouseId
        );

      if (wu == null) {
        throw new FailureOperation("该用户不存在");
      }

      return wu;
    }

  }
}
