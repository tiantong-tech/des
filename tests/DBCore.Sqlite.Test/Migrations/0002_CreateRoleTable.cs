using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite.Test
{
  public class CreateRoleTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("CreateRoleTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("DropRoleTable");
    }
  }
}
