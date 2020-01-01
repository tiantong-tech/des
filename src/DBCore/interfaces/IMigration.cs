namespace DBCore
{
  public interface IMigration
  {
    void Up(DbContext db);

    void Down(DbContext db);
  }
}
