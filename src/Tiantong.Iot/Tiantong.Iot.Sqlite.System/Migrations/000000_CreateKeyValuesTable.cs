using DBCore;

namespace Tiantong.Iot.Sqlite.System
{
  public class CreateKeyValuesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000000_CreateKeyValuesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists key_values");
    }
  }
}