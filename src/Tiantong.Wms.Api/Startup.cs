using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Renet.Web;
using DBCore;
using DBCore.Postgres;
using Tiantong.Wms.DB;

namespace Tiantong.Wms.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHttpContextAccessor();
      services.AddSingleton<PostgresBuilder, DbBuilder>();
      services.AddDbContext<DbContext>();
      services.AddScoped<MigratorProvider>();
      services.AddScoped<IAuth, Auth>();
      services.AddSingleton<IHash, Hash>();
      services.AddSingleton<IRandom, Random>();
      services.AddScoped<UserRepository>();
      services.AddScoped<WarehouseRepository>();
      services.AddScoped<KeeperRepository>();
      services.AddScoped<AreaRepository>();
      services.AddScoped<LocationRepository>();
      services.AddScoped<ProjectRepository>();
      services.AddScoped<SupplierRepository>();
      services.AddScoped<GoodCategoryRepository>();
      services.AddScoped<GoodRepository>();
      services.AddScoped<StockRepository>();
      services.AddScoped<OrderRepository>();
      services.AddScoped<OrderItemRepository>();
      services.AddScoped<ProjectItemRepository>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseProvider<ExceptionHandler>();
      app.UseMiddleware<JsonBody>();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseProvider<WebRoutes>();
    }
  }
}
