namespace DBCore.Sqlite
{
  public class Migrator : DBCore.Migrator
  {
    protected override void Initialize()
    {
      GetDbContext().ExecuteFromSql("CreateMigrationsTable");
    }

    protected override bool IsInitialized()
    {
      return false;
    }
  }
}
