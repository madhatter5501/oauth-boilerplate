using System;
using System.Linq;
using System.Reflection;
using Boilerplate.OAuth.Data;
using Boilerplate.OAuth.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.OAuth
{
    public class Startup
    {
        private readonly bool _useInMemoryStorage;
        
        public static IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            bool.TryParse(Configuration.GetSection("EnvironmentSettings")["UseInMemoryStorage"], out var useInMemoryStorage);
            _useInMemoryStorage = useInMemoryStorage;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_useInMemoryStorage)
            {
                services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryClients(Config.Clients())
                    .AddInMemoryApiResources(Config.ApiResources());
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

                services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddAspNetIdentity<ApplicationUser>()
                    .AddConfigurationStore(options =>
                    {
                        // Adds configuration data from the database (clients, resources)
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
                    .AddOperationalStore(options =>
                    {
                        // Adds operational data from the database (codes, tokens, consents)
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly));

                        // Enables automatic token cleanup
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30;
                    });
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!_useInMemoryStorage)
            {
                InitializeDatabase(app);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.ApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
