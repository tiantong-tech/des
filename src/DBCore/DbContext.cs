using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DBCore
{
  public class DbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Migration> Migrations { get; set; }

    public bool HasTable(string table)
    {
      try {
        Database.ExecuteSqlRaw($"select 1 from \"{table}\" limit 1");
        return true;
      } catch {
        return false;
      }
    }

    public bool HasTable<T>() where T : class
    {
      var set = this.Set<T>();

      try {
        set.ToList();
        return true;
      } catch {
        return false;
      }
    }

    public void ExecuteFromSql(string name)
    {
      var assembly = Assembly.GetCallingAssembly();
      var assemblyName = assembly.GetName().Name;
      var stream = assembly.GetManifestResourceStream($"{assemblyName}.Sql.{name}.sql");

      if (stream == null) {
        throw new Exception("Sql file not found");
      }

      var sreader = new StreamReader(stream);

      var sql = sreader.ReadToEnd();

      Database.ExecuteSqlRaw(sql);
    }
  }
}
