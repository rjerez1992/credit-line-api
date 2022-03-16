using CreditLineAPI.Middlewares;
using CreditLineAPI.Services;

namespace CreditLineAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSingleton<ICreditLineApplicationService, CreditLineApplicationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.MapWhen(context => context.Request.Path.StartsWithSegments("/credit-line/apply"), appBuilder =>
            {
                appBuilder.UseRateLimitMiddleware();
                appBuilder.UseMvc();
            });

            app.UseMvc();            
        }
    }
}
