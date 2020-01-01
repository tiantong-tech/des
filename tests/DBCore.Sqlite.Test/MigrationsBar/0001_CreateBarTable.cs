using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite.Test
{
  public class CreateBarTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("CreateBarTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("DropBarTable");
    }
  }
}
