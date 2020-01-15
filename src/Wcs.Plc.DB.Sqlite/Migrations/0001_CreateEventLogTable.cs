using DBCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class CreateEventLogTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.CreateEventLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.DropEventLogsTable");
    }
  }
}
