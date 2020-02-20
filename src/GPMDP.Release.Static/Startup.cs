using GPMDP.Release.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GPMDP.Release
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<CircleCIService>();
            services.AddSingleton<AppVeyorService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            // app.UseEmbeddedBlazorContent(typeof(MatBlazor.BaseMatComponent).Assembly);
            app.AddComponent<App>("app");
        }
    }
}
