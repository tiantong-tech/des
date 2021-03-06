using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.Utils;
using Midos.Web;
using Tiantong.Iot.Entities;
using Tiantong.Iot.Sqlite.Log;
using Tiantong.Iot.Sqlite.System;

namespace Tiantong.Iot.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddHostedService<PlcManagerService>();
      services.AddSingleton<Mail>();
      services.AddSingleton<Config>();
      services.AddSingleton<PlcManager>();
      services.AddSingleton<IHash>(app => new Hash(app.GetService<Config>().AppKey));
      services.AddSingleton<IRandom, Random>();
      services.AddSingleton<HttpPusherClient>();
      services.AddSingleton<DomainContextFactory>();
      services.AddScoped<LogContext, SqliteLogContext>();
      services.AddScoped<SystemContext, SqliteSystemContext>();
      services.AddScoped<HttpPusherRepository>();
      services.AddScoped<PlcRepository>();
      services.AddScoped<PlcLogRepository>();
      services.AddScoped<PlcStateRepository>();
      services.AddScoped<PlcStateLogRepository>();
      services.AddScoped<PlcStateErrorRepository>();
      services.AddScoped<PlcStateHttpPusherRepository>();
      services.AddScoped<SystemRepository>();
      services.AddTransient<PlcBuilder>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiddleware<JsonBody>();
      app.AddExceptionHandler();
      app.UseEmbeddedServer();
      app.UseRouting();
      app.UseCors(policy => policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
