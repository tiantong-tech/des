using DBCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class CreatePlcStateLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.CreatePlcStateLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.DropPlcStateLogsTable");
    }
  }
}
