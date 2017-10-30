using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecretsRUs.Models;
using SecretsRUs.Services;
using SecretsRUs.Services.Identity;
using SecretsRUs.Repositories;
using Microsoft.AspNetCore.Http;
using SecretsRUs.Services.Secrets;

namespace SecretsRUs
{
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
            /*
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            */

            // HttpAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add identity types
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            // Identity Services
            services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();

            // Repositories
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IIdentityRepository>(e => new IdentityRepository(connectionString));
            services.AddTransient<ISecretsRepository>(e => new SecretsRepository(connectionString));


            // TODO
            //services.AddTransient<DapperUsersTable>();

            // Add application services.
            //services.AddTransient<IEmailSender, EmailSender>();

            // Build up the UserVault
            // Built separely to allow for the UserId to be injected at creation via factory
            services.AddTransient<ISecretVault>(x =>
                new SecretVaultFactory(x.GetRequiredService<UserManager<ApplicationUser>>(),
                                        x.GetRequiredService<IHttpContextAccessor>(),
                                        x.GetRequiredService<ISecretsRepository>()).CreateVault()
            );

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

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
