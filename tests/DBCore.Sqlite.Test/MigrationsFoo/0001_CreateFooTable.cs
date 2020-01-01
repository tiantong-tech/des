using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite.Test
{
  public class CreateFooTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("CreateFooTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("DropFooTable");
    }
  }
}
