using Microsoft.EntityFrameworkCore;

namespace Midos.SeedWork.Domain
{
  public class EFContext: DbContext
  {
    private readonly EFContextOptions _options;

    public EFContext(EFContextOptions options)
    {
      _options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      _options.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      _options.OnModelCreating(builder);
    }
  }
}
