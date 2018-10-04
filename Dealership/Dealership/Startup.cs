namespace Dealership
{
    using Dealership.Data;
    using Dealership.Models;
    using Dealership.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DealershipDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DealershipDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            if (!DealershipDbContextExtentions.DatabaseExists(this.Configuration))
            {                
                using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<DealershipDbContext>();
                    var users = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                    var roles = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                    var config = scope.ServiceProvider.GetService<IConfiguration>();

                    context.Database.Migrate();

                    DealershipDbContextExtentions.EnsureDbSeeded(context, users, roles, config);
                }
            }

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Car}/{action=AllCars}/{id?}");
            });
        }
    }
}
