using DBCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class CreatePlcConnectionLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.CreatePlcConnectionLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.DropPlcConnectionLogsTable");
    }
  }
}
