using System.Linq;
using NUnit.Framework;

namespace DBCore.Sqlite.Test
{
  [TestFixture]
  public class TestMigrator
  {
    [Test]
    public void TestMigrateDefaultDirectory()
    {
      var db = new Database();
      var mg = new Migrator();

      mg.UseDbContext(db).Migrate();

      Assert.IsTrue(db.HasTable("users"));
      Assert.IsTrue(db.HasTable("roles"));
      Assert.IsFalse(db.HasTable("foo"));
    }

    [Test]
    public void TestRollback()
    {
      var db = new Database();
      var mg = new Migrator();

      mg.UseDbContext(db).Migrate();
      mg.Rollback();

      Assert.IsFalse(db.HasTable("users"));
    }

    [Test]
    public void TestRefresh()
    {
      var db = new Database();
      var mg = new Migrator();

      mg.UseDbContext(db).Migrate();
      mg.Refresh();

      db.Users.ToList();
      Assert.IsTrue(db.HasTable("users"));
    }

    [Test]
    public void TestUseMigrationDirectory()
    {
      var db = new Database();
      var mg = new Migrator();

      mg.UseDbContext(db).UseMigrationDirectory("MigrationsFoo").Migrate();

      Assert.IsTrue(db.HasTable("foo"));
      Assert.IsFalse(db.HasTable("users"));
    }

    [Test]
    public void TestMigrationOrder()
    {
      var db = new Database();
      var mg = new Migrator();

      mg.UseDbContext(db).UseMigrationDirectory("MigrationsBar").Migrate();

      Assert.IsTrue(db.HasTable("bar"));

      mg.Rollback();
    }

  }
}
