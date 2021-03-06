using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace DBCore
{
  using Migrations = SortedDictionary<string, IMigration>;

  public abstract class Migrator : IMigrator
  {
    private string _migrationDirectory = "Migrations";

    private Migrations _migrationInstances { get; set; }

    private Assembly _assembly { get => GetType().Assembly; }

    protected DbContext DbContext;

    public Migrator(DbContext db)
    {
      DbContext = db;
      _migrationInstances = GetMigrationInstances();
    }

    protected abstract void Initialize();

    protected virtual bool IsInitialized()
    {
      return DbContext.HasTable("_migrations");
    }

    private void EnsureInitialized()
    {
      if (!IsInitialized()) {
        Initialize();
      }
    }

    // 根据 _migrationDirectory 加载 instance
    // filename
    private Migrations GetMigrationInstances()
    {
      var dict = new Migrations();
      var assemblyName = _assembly.GetName().Name;
      var names = _assembly.GetManifestResourceNames();

      foreach (var name in names) {
        var filename = name.Substring(assemblyName.Length + 1);

        if (!filename.StartsWith(_migrationDirectory)) {
          continue;
        }

        filename = filename.Substring(_migrationDirectory.Length + 1);

        if (!filename.EndsWith(".cs")) {
          continue;
        }

        filename = filename.Substring(0, filename.Length - 3);

        var splits = filename.Split("_");
        var length = splits.Length;

        if (length < 2) {
          Helper.Warning($"migration file \"{filename}\" will be ignored because the name is invalid.\n");
          continue;
        }

        var output = 0;
        var id = splits[0];
        var typeName = splits[1];
        var type = _assembly.GetType(_assembly.GetName().Name + "." + typeName);

        if (type == null) {
          Helper.Warning($"migration file \"{filename}\" will be ignored because the type is not in assembly.\n");
          continue;
        }
        if (!int.TryParse(id, out output)) {
          Helper.Warning($"migration file \"{filename}\" will be ignored because the prefix of filename is not numeric.\n");
          continue;
        }
        if (!typeof(IMigration).IsAssignableFrom(type)) {
          Helper.Warning($"migration file \"{filename}\" will be ignored because the type is not implemented from interface \"IMigration\"\n");
          continue;
        }

        var mg = Activator.CreateInstance(type) as IMigration;

        dict.Add(filename, mg);
      }

      return dict;
    }

    public Migrator UseMigrationDirectory(string directory)
    {
      _migrationDirectory = directory;
      _migrationInstances = GetMigrationInstances();

      return this;
    }

    public int Migrate()
    {
      EnsureInitialized();

      var count = 0;
      var batchId = 1;
      var data = DbContext.Migrations.ToList();

      if (data.Count() > 0) {
        batchId = data.Max(mg => mg.BatchId) + 1;
      }

      DbContext.Database.BeginTransaction();

      foreach (var mg in _migrationInstances) {
        if (data.Exists(item => item.FileName == mg.Key)) {
          continue;
        }

        mg.Value.Up(DbContext);

        DbContext.Migrations.Add(new Migration() {
          FileName = mg.Key,
          BatchId = batchId,
          CreatedAt = DateTime.Now,
        });

        Helper.Success("migrated: ");
        Helper.Message($"{mg.Key}\n");
        count++;
      }

      DbContext.SaveChanges();
      DbContext.Database.CommitTransaction();

      Helper.Success("Database migrations completed successfully!\n");

      return count;
    }

    public int Rollback()
    {
      EnsureInitialized();

      var count = 0;
      var migrations = DbContext.Migrations.ToList();

      if (migrations.Count() == 0) return count;

      var batchId = migrations.Max(mg => mg.BatchId);

      migrations = migrations.Where(mg => mg.BatchId == batchId).Reverse().ToList();

      DbContext.Database.BeginTransaction();

      foreach (var mg in migrations) {
        if (!_migrationInstances.ContainsKey(mg.FileName)) {
          Helper.Error($"fail to drop, because \"{mg.FileName}\" file does not exist\n");
          continue;
        }

        _migrationInstances[mg.FileName].Down(DbContext);
        Helper.Warning("dropped: ");
        Helper.Message($"{mg.FileName}\n");
        count++;
      }

      var items = DbContext.Migrations.Where(mg => mg.BatchId == batchId).ToList();

      DbContext.Migrations.RemoveRange(items);
      DbContext.SaveChanges();
      DbContext.Database.CommitTransaction();

      return count;
    }

    public int Refresh()
    {
      EnsureInitialized();
      DbContext.Database.BeginTransaction();

      var migrations = DbContext.Migrations
        .OrderByDescending(mg => mg.FileName)
        .ToList();

      foreach (var mg in migrations) {
        if (!_migrationInstances.ContainsKey(mg.FileName)) {
          Helper.Error($"fail to drop, because {mg.FileName} does not exist\n");
          continue;
        }

        var ins = _migrationInstances[mg.FileName];

        ins.Down(DbContext);
        Helper.Warning($"dropped: ");
        Helper.Message($"{mg.FileName}\n");
      }

      DbContext.Migrations.RemoveRange(DbContext.Migrations);
      DbContext.SaveChanges();
      DbContext.Database.CommitTransaction();

      return Migrate();
    }
  }
}
