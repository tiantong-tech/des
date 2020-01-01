namespace DBCore
{
  public interface IMigrator
  {
    void Migrate();

    void Rollback();

    void Refresh();

  }
}
