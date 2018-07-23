using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Motohusaria.DataLayer;
using Motohusaria.DTO;
using Motohusaria.DomainClasses;
using Motohusaria.DataLayer.Repositories;
using Motohusaria.Services;
using Motohusaria.Services.Events;
using Motohusaria.Services.Utils.ScheduledTasks;
using Motohusaria.Services.Emailing;
using Motohusaria.Web.Utils.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Motohusaria.Web
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
        services.AddMvc(option => option.Filters.Add<ExceptionLoggingFilter>()).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        RegisterDependencies(services);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            var jwtOptions = Configuration.GetSection("JWT");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.GetValue<string>("Secret"))),
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.GetValue<string>("Issuer"),
                ValidateAudience = true,
                ValidAudience = jwtOptions.GetValue<string>("Audience"),
                ValidateLifetime = true, //validate the expiration and not before values in the token
                ClockSkew = TimeSpan.FromSeconds(5) //5 minute tolerance for the expiration date
            };
        });
        services.Configure<Utils.Authorization.JWTOptions>(Configuration.GetSection("JWT"));
        services.Configure<EmailingConfig>(Configuration.GetSection("EmailingConfig"));
    }

    private void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
        var exportedTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.ExportedTypes);
        var types = exportedTypes
                  .Select(s => new { Type = s, Attr = s.GetCustomAttribute<InjectableServiceAttribute>() })
                  .Where(w => w.Attr != null);
        foreach (var type in types)
        {
            services.AddScoped(type.Attr.Type, type.Type);
        }

        var entites = AppDomain.CurrentDomain.GetAssemblies().Where(w => !w.IsDynamic)
            .SelectMany(s => s.ExportedTypes)
            .Where(w => w.BaseType == typeof(BaseEntity));
        foreach (var entity in entites)
        {
            services.AddScoped(typeof(IRepository<>).MakeGenericType(entity), typeof(GenericRepository<>).MakeGenericType(entity));
        }
        services.AddDbContext <MotohusariaDbContext > (options => options.UseSqlServer(Configuration.GetConnectionString("Default")).UseLazyLoadingProxies());
        services.AddDbContext<LoggingDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Logging"), x => x.MigrationsHistoryTable(LoggingDbContext.MigrationsHistoryTable, LoggingDbContext.Schema)));

        services.AddScoped<ICacheService, MemoryCacheService>();
        services.AddScoped<DataLayer.Utils.ITransactionProvider, DataLayer.Utils.TransactionProvider>();
        var config = new MapperConfiguration(x =>
        {
            x.AddProfile(new Utils.AutomapperConfig());
        });
        // In production, the Angular files will be served from this directory
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/dist";
        });
        services.AddScoped<IMapper>(ctx => config.CreateMapper());
        foreach (var type in exportedTypes)
        {
            var interfaces = type.GetInterfaces();
            var subscribeInterfaces = interfaces.Where(a => a.IsGenericType && typeof(IEventSubscriber<>).IsAssignableFrom(a.GetGenericTypeDefinition()));
            foreach (var sub in subscribeInterfaces)
            {
                services.AddScoped(sub, type);
            }
        }
        //scheduledTasks
        var scheduledTasks = exportedTypes.Where(w => !w.IsInterface && !w.IsAbstract && typeof(IScheduledTask).IsAssignableFrom(w)).ToArray();
        foreach (var scheduledTask in scheduledTasks)
        {
            services.AddSingleton(typeof(IHostedService), typeof(TaskScheduler<>).MakeGenericType(scheduledTask));
            services.AddScoped(scheduledTask);
        }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseAuthentication();
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
        });

        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.UseAngularCliServer(npmScript: "start");
            }
        });
    }
}
}
