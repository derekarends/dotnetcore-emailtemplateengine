using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Templates;

namespace Api
{
  public class Startup
  {
    private readonly IConfiguration _configuration;
    
    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseMvc();
    }
  }
}