using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace DBCore
{
  using Migrations = SortedDictionary<string, IMigration>;

  public abstract class Migrator : IMigrator, IDisposable
  {
    private string _migrationDirectory = "Migrations";

    private Migrations _migrationInstances { get; set; }

    private Assembly _assembly;

    private DbContext _dbContext;

    public Migrator()
    {
      _assembly = GetAssembly();
      _migrationInstances = GetMigrationInstances();
    }

    public void Dispose()
    {
      _dbContext.Dispose();
    }

    protected abstract void Initialize();

    protected abstract bool IsInitialized();

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
      var names = _assembly.GetManifestResourceNames();

      foreach (var name in names) {
        // Assembly.Directory.Filename.suffix
        var splits = name.Split(".");
        var length = splits.Length;

        if (splits.Length <= 4) continue;

        var suffix = splits[length - 1];
        var filename = splits[length - 2];
        var dir = splits[length - 3];

        if (suffix != "cs" || dir != _migrationDirectory) continue;

        splits = filename.Split("_");
        length = splits.Length;

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

    protected virtual Assembly GetAssembly()
    {
      return this.GetType().Assembly;
    }

    protected virtual DbContext GetDbContext()
    {
      if (_dbContext == null) {
        var type = _assembly.GetTypes()
          .Where(t => typeof(DbContext).IsAssignableFrom(t))
          .First();

        _dbContext = (DbContext) Activator.CreateInstance(type);
      }

      return _dbContext;
    }

    public Migrator UseDbContext(DbContext dbContext)
    {
      _dbContext = dbContext;

      return this;
    }

    public Migrator UseMigrationDirectory(string directory)
    {
      _migrationDirectory = directory;
      _migrationInstances = GetMigrationInstances();

      return this;
    }

    public void Migrate()
    {
      EnsureInitialized();

      var dbcontext = GetDbContext();
      var data = dbcontext.Migrations.ToList();
      var batchId = 1;

      if (data.Count() > 0) {
        batchId = data.Max(mg => mg.BatchId) + 1;
      }

      dbcontext.Database.BeginTransaction();

      foreach (var mg in _migrationInstances) {
        if (data.Exists(item => item.FileName == mg.Key)) {
          continue;
        }

        mg.Value.Up(dbcontext);

        dbcontext.Migrations.Add(new Migration() {
          FileName = mg.Key,
          BatchId = batchId,
          CreatedAt = DateTime.Now,
        });

        Helper.Success("migrated: ");
        Helper.Message($"{mg.Key}\n");
      }

      dbcontext.SaveChanges();
      dbcontext.Database.CommitTransaction();

      Helper.Success("Database migrations completed successfully!\n");
    }

    public void Rollback()
    {
      EnsureInitialized();

      var dbcontext = GetDbContext();
      var migrations = dbcontext.Migrations.ToList();

      if (migrations.Count() == 0) return;

      var batchId = migrations.Max(mg => mg.BatchId);

      migrations = migrations.Where(mg => mg.BatchId == batchId).Reverse().ToList();

      dbcontext.Database.BeginTransaction();

      foreach (var mg in migrations) {
        if (!_migrationInstances.ContainsKey(mg.FileName)) {
          Helper.Error($"fail to drop, because \"{mg.FileName}\" file does not exist\n");
          continue;
        }

        _migrationInstances[mg.FileName].Down(dbcontext);
        Helper.Warning("dropped: ");
        Helper.Message($"{mg.FileName}\n");
      }

      dbcontext.Migrations.Where(mg => mg.BatchId == batchId);
      dbcontext.SaveChanges();
      dbcontext.Database.CommitTransaction();
    }

    public void Refresh()
    {
      EnsureInitialized();

      var dbcontext = GetDbContext();

      dbcontext.Database.BeginTransaction();

      var migrations = dbcontext.Migrations
        .OrderByDescending(mg => mg.FileName)
        .ToList();

      foreach (var mg in migrations) {
        if (!_migrationInstances.ContainsKey(mg.FileName)) {
          Helper.Error($"fail to drop, because {mg.FileName} does not exist\n");
          continue;
        }

        var ins = _migrationInstances[mg.FileName];

        ins.Down(dbcontext);
        Helper.Warning($"dropped: ");
        Helper.Message($"{mg.FileName}\n");
      }

      dbcontext.Migrations.RemoveRange(dbcontext.Migrations);
      dbcontext.SaveChanges();
      dbcontext.Database.CommitTransaction();

      Migrate();
    }

  }
}
