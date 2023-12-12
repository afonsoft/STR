using System.IO;
using Microsoft.AspNetCore.Hosting;
using Eaf.Middleware.Web.Serilog;
using Eaf.Middleware.Web.Configuration;
using System.Threading;
using System;
using System.Globalization;
using Serilog;

namespace Eaf.Str.Web.Startup
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal Error in Main : {0}", ex.Message);
                Environment.Exit(1);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEafSerilog()
                .UseEafConfiguration(prefix: "ProjectName_")
                .UseKestrel(opt =>
                {
                    opt.AddServerHeader = false;
                    opt.Limits.MaxRequestLineSize = 16 * 1024;
                })
               .UseIISIntegration()
               /* .UseIIS() //For Windows IIS */
               .UseStartup<Startup>();
        }
    }
}