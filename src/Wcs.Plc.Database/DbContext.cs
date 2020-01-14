using Wcs.Plc.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wcs.Plc.Database
{
  public class DbContext : DBCore.DbContext
  {
    public DbSet<EventLog> EventLogs { get; set; }
  }
}
