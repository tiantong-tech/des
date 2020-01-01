using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite.Test
{
  public class CreateUserTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("CreateUserTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("DropUserTable");
    }
  }
}
