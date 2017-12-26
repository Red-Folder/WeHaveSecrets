using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeHaveSecrets.Models;
using WeHaveSecrets.Services;
using WeHaveSecrets.Services.Identity;
using WeHaveSecrets.Repositories;
using Microsoft.AspNetCore.Http;
using WeHaveSecrets.Services.Secrets;
using System.IO;
using Microsoft.Extensions.FileProviders;
using WeHaveSecrets.Services.Social;

namespace WeHaveSecrets
{
    public class Startup
    {
        private readonly string _backupPath;
        private readonly string _backupFolder;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _backupFolder = "/backups";
            _backupPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "backups");
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
            services.AddTransient<IUserPasswordStore<ApplicationUser>, CustomUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();

            // Repositories
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IIdentityRepository>(e => new IdentityRepository(connectionString));
            services.AddTransient<ISecretsRepository>(e => new SecretsRepository(connectionString));
            services.AddTransient<IMaintenanceRepository>(e => new MaintenanceRepository(connectionString));
            services.AddTransient<ITestimonialRepository>(e => new TestimonialRepository(connectionString));

            services.AddTransient<ISocialProof, SocialProof>();
            services.AddTransient<IDatabaseMaintenance>(e => new DatabaseMaintenance(_backupFolder, _backupPath, e.GetRequiredService<IMaintenanceRepository>()));
            services.AddTransient<IAdminVault, AdminVault>();

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

            services.AddDirectoryBrowser();

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

            if (!Directory.Exists(_backupPath))
            {
                Directory.CreateDirectory(_backupPath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(_backupPath),
                ServeUnknownFileTypes = true,
                RequestPath = new PathString(_backupFolder)
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(_backupPath),
                RequestPath = new PathString(_backupFolder)
            });

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
