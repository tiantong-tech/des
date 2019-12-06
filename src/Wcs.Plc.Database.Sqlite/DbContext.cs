using System.Data.Common;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wcs.Plc.Entities;

namespace Wcs.Plc.Database.Sqlite
{
  public class SqliteDbContext : DbContext
  {
    public DbSet<EventLog> EventLogs { get; set; }

    static bool _isInMemory = false;

    static bool _isInMemoryMigrated = false;

    static SqliteDbContext _DB;

    static public SqliteDbContext NewDB()
    {
      if (_isInMemory) {
        return GetDB();
      } else {
        return new SqliteDbContext();
      }
    }

    static public SqliteDbContext GetDB()
    {
      if (_DB == null) {
        _DB = new SqliteDbContext();
      }

      return _DB;
    }

    static public void InMemory(bool isInMemory = true)
    {
      _isInMemory = isInMemory;
    }

    public SqliteDbContext()
    {
      if (_isInMemory && !_isInMemoryMigrated) {
        Database.OpenConnection();
        Database.EnsureCreated();
      }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      if (_isInMemory) {
        options.UseSqlite("DataSource=:memory:");
      } else {
        options.UseSqlite("DataSource=./sqlite.db");
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<EventLog>()
        .Property(b => b.CreatedAt)
        .HasDefaultValue(DateTime.Now);
    }
  }
}
