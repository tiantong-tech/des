﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tiantong.Iot.Entities
{
  public class IotDbContext : DBCore.DbContext
  {
    public DbSet<Plc> Plcs { get; set; }

    public DbSet<PlcLog> PlcLogs { get; set; }

    public DbSet<PlcError> PlcErrors { get; set; }

    public DbSet<PlcState> PlcStates { get; set; }

    public DbSet<PlcStateLog> PlcStateLogs { get; set; }

    public DbSet<PlcStateError> PlcStateErrors { get; set; }

    public DbSet<PlcStateHttpPusher> PlcStateHttpPushers { get; set; }

    public DbSet<HttpPusher> HttpPushers { get; set; }

    public DbSet<HttpPusherLog> HttpPusherLogs { get; set; }

    public DbSet<HttpPusherError> HttpPusherErrors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
  }
}
