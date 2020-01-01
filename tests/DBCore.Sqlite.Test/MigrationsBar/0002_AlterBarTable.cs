using Microsoft.EntityFrameworkCore;

namespace DBCore.Sqlite.Test
{
  public class AlterBarTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("AlterBarTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("DropAlterBarTable");
    }
  }
}
