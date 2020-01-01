using NUnit.Framework;

namespace DBCore.Sqlite.Test
{
  [TestFixture]
  public class DbContextTest
  {
    [Test]
    public void Test()
    {
      var db = new Database();
      var mg = new Migrator();

      mg.UseDbContext(db).Migrate();

      Assert.IsTrue(db.HasTable<User>());
      Assert.IsFalse(db.HasTable<Role>());

      Assert.IsTrue(db.HasTable("users"));
      Assert.IsTrue(db.HasTable("roles"));
      Assert.IsFalse(db.HasTable("foo"));
    }

  }
}
