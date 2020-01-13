using DBCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class CreateEventLogTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.CreateEventLogTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.DropEventLogTable");
    }
  }
}
