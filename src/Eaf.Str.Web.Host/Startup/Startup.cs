using Castle.Facilities.Logging;
using Eaf.AspNetCore;
using Eaf.AspNetCore.Configuration;
using Eaf.AspNetCore.Hangfire.Configuration;
using Eaf.AspNetCore.Mvc.Antiforgery;
using Eaf.AspNetCore.SignalR.Chat;
using Eaf.AspNetCore.SignalR.Hubs;
using Eaf.Castle.Logging.SerilogIntegration;
using Eaf.Middleware.Configuration;
using Eaf.Middleware.Swagger;
using Eaf.Middleware.Web.Authentication.JwtBearer;
using Eaf.Middleware.Web.Serilog;
using Eaf.Middleware.Web.Startup;
using Eaf.Middleware.Web.Swagger;
using Eaf.PlugIns;
using Eaf.Str.Application.Extensions;
using Eaf.Str.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Eaf.Str.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new EafAutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add<SerilogMvcLoggingAttribute>();
                options.Filters.Add(new ResponseCacheAttribute() { NoStore = true, Location = ResponseCacheLocation.None });
            }).AddNewtonsoftJson();

            services.AddEafConfigurer(_appConfiguration);

            //Configure OpenTelemetry
            services.AddEafOpenTelemetry(options =>
            {
                options.ServiceName = "Str";
                options.SourceName = new[]
                {
                    "Eaf.Str.Web.Host",
                    "Eaf.Str.EntityFrameworkCore",
                    "Eaf.Str.Core",
                    "Eaf.Str.Application"
                };
            });

            //Configure CORS for angular2 UI
            services.AddCors(options =>
            {
                options.AddPolicy(StrConsts.DefaultCorsPolicyName, builder =>
                {
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains();

                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    if (_appConfiguration["App:CorsOrigins"] == "*")
                        builder
                            .SetIsOriginAllowed((host) => true);
                    else
                        builder.WithOrigins(
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        );

                    builder.AllowAnyMethod()
                           .AllowCredentials()
                           .AllowAnyHeader()
                           .Build();
                });
            });

            //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Str API",
                    Description = "Sistema de transporte e rastreamento",
                    Contact = new OpenApiContact
                    {
                        Name = "Transportation and tracking system",
                        Email = "contato@afonsoft.com.br"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License"
                    }
                });

                options.DocInclusionPredicate((docName, description) => true);
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.CustomSchemaIds(type => type.FullName);
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
                options.ParameterFilter<SwaggerEnumParameterFilter>();
                options.ParameterFilter<SwaggerNullableParameterFilter>();
                options.SchemaFilter<SwaggerEnumSchemaFilter>();
                options.OperationFilter<SwaggerOperationIdFilter>();
                options.OperationFilter<SwaggerOperationFilter>();
                options.CustomDefaultSchemaIdSelector();
                options.SupportNonNullableReferenceTypes();
            }).AddSwaggerGenNewtonsoftSupport();

            //Configure Eaf and Dependency Injection
            return services.AddEaf<WebHostModule>(options =>
            {
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseEafSerilog()
                );
                options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"), SearchOption.AllDirectories);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //Initializes Eaf framework.
            app.UseEaf(options =>
            {
                options.UseEafRequestLocalization = false;
            });

            app.UseEafHealthChecks();

            app.UseMiddleware<ContentSecurityPolicyMiddleware>();

            app.UseDeveloperExceptionPage();

            app.UseCors(StrConsts.DefaultCorsPolicyName); //Enable CORS!

            app.UseJwtTokenMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<EafCommonHub>("/signalr");
                endpoints.MapHub<ChatHub>("/signalr-chat");

                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                app.ApplicationServices.GetRequiredService<IEafAspNetCoreConfiguration>().EndpointConfiguration.ConfigureAllEndpoints(endpoints);
            });

            // Enable middleware HangFire
            app.UseEafHangfire(opt => opt.IsEnabled = false);

            //For Security Only Swagger on Develop/Staging
            if (!_hostingEnvironment.IsProduction())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();
                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("../swagger/v1/swagger.json", "Str API V1");
                    options.IndexStream = () => Assembly.GetExecutingAssembly().GetManifestResourceStream("Eaf.Str.Web.wwwroot.swagger.ui.index.html");
                    options.InjectBaseUrl(_appConfiguration["App:ServerRootAddress"]);
                });
            }

            //All Recurring Jobs
            app.ScheduleRecurringJobs();
        }
    }
}