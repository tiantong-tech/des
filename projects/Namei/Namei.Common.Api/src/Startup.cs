using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Midos.Services.Http;

namespace Namei.Common.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExceptionHandler();
      services.AddControllers();
      services.AddHttpClient();
      services.AddSingleton<Config>();
      services.AddSingleton<IHttpService, HttpService>();
      services.AddDbContext<RcsContext>();
      services.AddSingleton<RcsHttpService>();
      services.AddDbContext<SapContext>();
      services.AddDbContext<WmsContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.AddExceptionHandler();
      app.UseMiddleware<JsonBody>();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
  }
}
